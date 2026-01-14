using TheButton.Api.Abstractions;

namespace TheButton.Api.Features.Health.GetHealth;

public static class Endpoint
{
    public static RouteGroupBuilder MapHealthEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", () => Results.Ok(new { Status = "Healthy" }))
             .WithName("GetHealth");
        return group;
    }
}
