using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheButton.Infrastructure.Persistence;

namespace TheButton.Api.IntegrationTests.Features.V3;

[TestClass]
public class UnifiedCounterTests : IntegrationTestBase
{
    private HttpClient _client = null!;

    [ClassInitialize]
    public static void SetupTests(TestContext context)
    {
        IntegrationTestBase.SetupAsync().GetAwaiter().GetResult();
    }

    [ClassCleanup]
    public static void CleanupTests()
    {
        IntegrationTestBase.TeardownAsync().GetAwaiter().GetResult();
    }

    [TestInitialize]
    public void Init()
    {
        _client = Factory.CreateClient();
    }

    [TestMethod]
    public async Task PostGlobal_IncrementsAndPersistsEvent()
    {
        // Act
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v3/counter");
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());
        var response = await _client.SendAsync(request);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Request failed with {response.StatusCode}. Content: {error}");
        }
        
        var result = await response.Content.ReadFromJsonAsync<CounterResponse>();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.GlobalValue > 0);

        // Verify DB
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();
        
        var eventCount = await db.Database.SqlQueryRaw<int>("SELECT COUNT(*) as Value FROM write.Events WHERE UserId IS NULL").SingleAsync();
        Assert.AreEqual(1, eventCount);
    }

    [TestMethod]
    public async Task PostUser_IncrementsAndPersistsEvent()
    {
        var userId = Guid.NewGuid();

        // Act
        using var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v3/counter?userId={userId}");
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CounterResponse>();
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.UserValue);

        // Verify DB
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();

        var userEvent = await db.Database.SqlQueryRaw<long>(
            "SELECT TOP 1 UserVersion as Value FROM write.Events WHERE UserId = {0}", userId).SingleAsync();
        Assert.AreEqual(1, userEvent);
    }

    [TestMethod]
    public async Task Idempotency_PreventsDoubleIncrement()
    {
        var key = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid();

        // Act 1
        using var req1 = new HttpRequestMessage(HttpMethod.Post, $"/api/v3/counter?userId={userId}");
        req1.Headers.Add("Idempotency-Key", key);
        var resp1 = await _client.SendAsync(req1);
        resp1.EnsureSuccessStatusCode();
        var json1 = await resp1.Content.ReadAsStringAsync();

        // Act 2
        using var req2 = new HttpRequestMessage(HttpMethod.Post, $"/api/v3/counter?userId={userId}");
        req2.Headers.Add("Idempotency-Key", key);
        var resp2 = await _client.SendAsync(req2);
        resp2.EnsureSuccessStatusCode();
        var json2 = await resp2.Content.ReadAsStringAsync();

        // Assert
        Assert.AreEqual(json1, json2);

        // Verify DB (only 1 event)
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();
        
        var eventCount = await db.Database.SqlQueryRaw<int>(
            "SELECT COUNT(*) as Value FROM write.Events WHERE UserId = {0}", userId).SingleAsync();
        Assert.AreEqual(1, eventCount);
    }

    [TestMethod]
    public async Task Idempotency_ScopedByOperationAndUser()
    {
        var key = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid();
        
        // Act 1: User increment
        using var reqUser = new HttpRequestMessage(HttpMethod.Post, $"/api/v3/counter?userId={userId}");
        reqUser.Headers.Add("Idempotency-Key", key);
        var respUser = await _client.SendAsync(reqUser);
        respUser.EnsureSuccessStatusCode();

        // Act 2: Global increment (same key, different "user context" i.e. null)
        using var reqGlobal = new HttpRequestMessage(HttpMethod.Post, "/api/v3/counter");
        reqGlobal.Headers.Add("Idempotency-Key", key);
        var respGlobal = await _client.SendAsync(reqGlobal);
        respGlobal.EnsureSuccessStatusCode();

        // Assert: Both succeeded and created distinct events
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();

        var userCount = await db.Database.SqlQueryRaw<int>("SELECT COUNT(*) as Value FROM write.Events WHERE UserId = {0}", userId).SingleAsync();
        var globalCount = await db.Database.SqlQueryRaw<int>("SELECT COUNT(*) as Value FROM write.Events WHERE UserId IS NULL").SingleAsync();

        Assert.AreEqual(1, userCount);
        Assert.AreEqual(1, globalCount);
    }

    [TestMethod]
    public async Task Concurrency_ParallelUserIncrements_AreSequential()
    {
        var userId = Guid.NewGuid();
        int parallelCount = 10;
        
        var tasks = new List<Task>();
        
        for (int i = 0; i < parallelCount; i++)
        {
            tasks.Add(Task.Run(async () => 
            {               
                var key = Guid.NewGuid().ToString();
                using var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v3/counter?userId={userId}");
                request.Headers.Add("Idempotency-Key", key);
                var r = await _client.SendAsync(request);
                if (!r.IsSuccessStatusCode)
                {
                    var error = await r.Content.ReadAsStringAsync();
                    throw new Exception($"Request failed with {r.StatusCode}. Content: {error}");
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Verify DB
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();

        var maxVersion = await db.Database.SqlQueryRaw<long>(
            "SELECT MAX(UserVersion) as Value FROM write.Events WHERE UserId = {0}", userId).SingleAsync();
        
        Assert.AreEqual(10, maxVersion);
    }

    [TestMethod]
    public async Task Concurrency_ParallelGlobalIncrements_AdvancePosition()
    {
        int parallelCount = 10;
        var tasks = new List<Task>();

        for (int i = 0; i < parallelCount; i++)
        {
            tasks.Add(Task.Run(async () => 
            {
                var key = Guid.NewGuid().ToString();
                using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v3/counter");
                request.Headers.Add("Idempotency-Key", key);
                var r = await _client.SendAsync(request);
                r.EnsureSuccessStatusCode();
            }));
        }

        await Task.WhenAll(tasks);

        // Verify DB
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();

        var count = await db.Database.SqlQueryRaw<int>(
            "SELECT COUNT(*) as Value FROM write.Events WHERE UserId IS NULL").SingleAsync();
        
        Assert.AreEqual(parallelCount, count);
    }

    record CounterResponse(long GlobalValue, long? UserValue);
}
