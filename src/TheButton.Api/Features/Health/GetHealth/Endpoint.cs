using TheButton.Api.Abstractions;

namespace TheButton.Api.Features.Health.GetHealth;

public class Endpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }))
           .WithTags("Health")
           .WithName("GetHealth");
    }
}
