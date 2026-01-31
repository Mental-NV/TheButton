using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheButton.Mobile.Core;
using TheButton.Mobile.Infrastructure.V3;

namespace TheButton.Mobile.Infrastructure;

/// <summary>
/// Dependency injection helpers for mobile infrastructure services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers mobile infrastructure dependencies.
    /// </summary>
    /// <param name="services">The service collection to register with.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated service collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="configuration"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the base API URL is missing from configuration.</exception>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        string? baseApiUrl = configuration[Constants.BaseApiUrlSection];

        if (string.IsNullOrWhiteSpace(baseApiUrl))
        {
            throw new InvalidOperationException($"Configuration missing required key: {Constants.BaseApiUrlSection}");
        }

        if (!baseApiUrl.EndsWith('/'))
        {
            baseApiUrl = $"{baseApiUrl}/";
        }

        _ = services.AddHttpClient<ICounterApiClient, CounterApiV3Client>(
            client => client.BaseAddress = new Uri(baseApiUrl, UriKind.Absolute));

        return services;
    }
}
