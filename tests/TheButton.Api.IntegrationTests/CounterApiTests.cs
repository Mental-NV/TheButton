using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using TheButton.Api.Models;

namespace TheButton.Api.IntegrationTests;

[TestClass]
public class CounterApiTests
{
    private static WebApplicationFactory<Program> _factory = null!;

    [ClassInitialize]
    public static void ClassInit(TestContext context)
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }

    [TestMethod]
    public async Task Increment_V2_ReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/api/v2/counter", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task Increment_V2_IncrementsValue()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act - First Increment
        var response1 = await client.PostAsync("/api/v2/counter", null);
        response1.EnsureSuccessStatusCode();
        var result1 = await response1.Content.ReadFromJsonAsync<CounterResponse>();

        // Act - Second Increment
        var response2 = await client.PostAsync("/api/v2/counter", null);
        response2.EnsureSuccessStatusCode();
        var result2 = await response2.Content.ReadFromJsonAsync<CounterResponse>();

        // Assert
        Assert.IsNotNull(result1);
        Assert.IsNotNull(result2);
        Assert.IsGreaterThan(result1.Value, result2.Value, "Counter should increment between calls.");
    }

    [TestMethod]
    public async Task Click_V1_VersionedRoute_ReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/api/v1/button/click", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
