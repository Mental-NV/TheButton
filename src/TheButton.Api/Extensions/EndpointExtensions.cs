using TheButton.Api.Features.Health.GetHealth;
using TheButton.Api.Features.V2.Counter;
using TheButton.Api.Features.V3.Counter;

namespace TheButton.Api.Extensions;

public static class EndpointExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api");

        // V2
        api.MapGroup("/v2/counter")
           .WithTags("Counter V2")
           .MapV2CounterEndpoints();

        // V3
        api.MapGroup("/v3/counter")
           .WithTags("Counter V3")
           .MapV3CounterEndpoints();
           
        // Health
        app.MapGroup("/health")
           .WithTags("Health")
           .MapHealthEndpoints();
    }
}
