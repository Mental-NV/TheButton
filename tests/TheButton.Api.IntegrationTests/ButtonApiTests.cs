using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using TheButton.Api.Models;

namespace TheButton.Api.IntegrationTests;

[TestClass]
public class ButtonApiTests
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
    public async Task Click_Endpoint_ReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/api/button/click", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task Click_Endpoint_IncrementsValue()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act - First Click
        var response1 = await client.PostAsync("/api/button/click", null);
        response1.EnsureSuccessStatusCode();
        var result1 = await response1.Content.ReadFromJsonAsync<CounterResponse>();

        // Act - Second Click
        var response2 = await client.PostAsync("/api/button/click", null);
        response2.EnsureSuccessStatusCode();
        var result2 = await response2.Content.ReadFromJsonAsync<CounterResponse>();

        // Assert
        Assert.IsNotNull(result1);
        Assert.IsNotNull(result2);
        Assert.IsTrue(result2.Value > result1.Value, $"Expected {result2.Value} to be greater than {result1.Value}");
        // Note: Since this is an integration test running against a shared singleton service in memory,
        // we can't assert exact values easily unless we reset the state or have isolation.
        // For now, asserting it increases is sufficient.
    }
}
