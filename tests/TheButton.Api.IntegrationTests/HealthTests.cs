using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheButton.Api.IntegrationTests;

[TestClass]
public class HealthTests : IntegrationTestBase
{
    [ClassInitialize]
    public static async Task ClassInit(TestContext context)
    {
        await SetupAsync();
    }

    [ClassCleanup]
    public static async Task ClassCleanup()
    {
        await TeardownAsync();
    }

    [TestMethod]
    public async Task GetLiveHealth_ReturnsOk()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/live");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task GetReadyHealth_ReturnsOk()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
