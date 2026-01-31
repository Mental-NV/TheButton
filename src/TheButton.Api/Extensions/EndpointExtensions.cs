using TheButton.Api.Features.Health.GetHealth;
using TheButton.Api.Features.V2.Counter;
using TheButton.Api.Features.V3.Counter;

namespace TheButton.Api.Extensions;

/// <summary>
/// Maps application endpoints onto the web application.
/// </summary>
internal static class EndpointExtensions
{
    /// <summary>
    /// Maps all API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void MapEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        RouteGroupBuilder api = app.MapGroup("/api");

        // V2
        _ = api.MapGroup("/v2/counter")
            .WithTags("Counter V2")
            .MapV2CounterEndpoints();

        // V3
        _ = api.MapGroup("/v3/counter")
            .WithTags("Counter V3")
            .MapV3CounterEndpoints();

        // Health
        _ = app.MapGroup("/health")
            .WithTags("Health")
            .MapHealthEndpoints();
    }
}
