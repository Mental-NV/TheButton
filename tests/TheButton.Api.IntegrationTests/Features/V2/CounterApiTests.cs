using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using TheButton.Api.Features.V2.Counter;

namespace TheButton.Api.IntegrationTests.Features.V2;

[TestClass]
public class CounterApiTests
{
    private static WebApplicationFactory<Program> _factory = null!;

    [ClassInitialize]
    public static void ClassInit(TestContext context)
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
            });
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }

    [TestMethod]
    public async Task Post_Increment_V2_ReturnsUpdatedValue()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response1 = await client.PostAsync("/api/v2/counter", null);
        response1.EnsureSuccessStatusCode();
        var result1 = await response1.Content.ReadFromJsonAsync<CounterResponse>();

        var response2 = await client.PostAsync("/api/v2/counter", null);
        response2.EnsureSuccessStatusCode();
        var result2 = await response2.Content.ReadFromJsonAsync<CounterResponse>();

        // Assert
        Assert.IsNotNull(result1);
        Assert.IsNotNull(result2);
        Assert.IsTrue(result2.Value > result1.Value, "V2 counter should increment.");
    }
}
