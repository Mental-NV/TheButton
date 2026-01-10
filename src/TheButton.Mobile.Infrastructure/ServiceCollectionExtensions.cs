using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheButton.Mobile.Core;

namespace TheButton.Mobile.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var baseApiUrl = configuration[Constants.BaseApiUrlSection];
        
        // Ensure trailing slash and valid URI
        if (string.IsNullOrWhiteSpace(baseApiUrl))
        {
            throw new InvalidOperationException($"Configuration missing required key: {Constants.BaseApiUrlSection}");
        }

        if (!baseApiUrl.EndsWith("/"))
        {
            baseApiUrl += "/";
        }

        services.AddHttpClient<ICounterApiClient, CounterApiClient>(client =>
        {
            client.BaseAddress = new Uri(baseApiUrl);
        });

        return services;
    }
}
