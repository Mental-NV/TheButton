using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace TheButton.Api.IntegrationTests;

[TestClass]
public class ProgramConfigurationTests
{
    private static WebApplicationFactory<Program> CreateFactory(
        string environment,
        string connectionString,
        string? allowedOrigin = null)
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment(environment);
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    var settings = new Dictionary<string, string?>
                    {
                        { "ConnectionStrings:Sql", connectionString }
                    };

                    if (!string.IsNullOrWhiteSpace(allowedOrigin))
                    {
                        settings["AllowedOrigins:0"] = allowedOrigin;
                    }

                    config.AddInMemoryCollection(settings);
                });
            });
    }

    [TestMethod]
    public async Task OpenApi_IsAvailable_InDevelopment()
    {
        var dbName = $"TheButton_Test_{Guid.NewGuid()}";
        var connectionString = $@"Server=(localdb)\MSSQLLocalDB;Database={dbName};Trusted_Connection=True;MultipleActiveResultSets=True";

        using var factory = CreateFactory("Development", connectionString);
        var client = factory.CreateClient();

        var response = await client.GetAsync("/openapi/v1.json");

        response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task Cors_Allows_Configured_Origin()
    {
        var dbName = $"TheButton_Test_{Guid.NewGuid()}";
        var connectionString = $@"Server=(localdb)\MSSQLLocalDB;Database={dbName};Trusted_Connection=True;MultipleActiveResultSets=True";
        const string allowedOrigin = "https://example.com";

        using var factory = CreateFactory("Testing", connectionString, allowedOrigin);
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/health");
        request.Headers.Add("Origin", allowedOrigin);

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        Assert.IsTrue(response.Headers.TryGetValues("Access-Control-Allow-Origin", out var values));
        Assert.IsTrue(values.Contains(allowedOrigin));
    }

    [TestMethod]
    public async Task Cors_Rejects_Unconfigured_Origin()
    {
        var dbName = $"TheButton_Test_{Guid.NewGuid()}";
        var connectionString = $@"Server=(localdb)\MSSQLLocalDB;Database={dbName};Trusted_Connection=True;MultipleActiveResultSets=True";
        const string allowedOrigin = "https://example.com";

        using var factory = CreateFactory("Testing", connectionString, allowedOrigin);
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/health");
        request.Headers.Add("Origin", "https://not-allowed.example");

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        Assert.IsFalse(response.Headers.Contains("Access-Control-Allow-Origin"));
    }
}
