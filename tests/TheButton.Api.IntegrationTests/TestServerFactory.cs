using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace TheButton.Api.IntegrationTests;

public class TestServerFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public TestServerFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var settings = new Dictionary<string, string?>
            {
                { "ConnectionStrings:Sql", _connectionString }
            };

            config.AddInMemoryCollection(settings);
        });
    }
}
