namespace TheButton.Api.Features.Health.GetHealth;

/// <summary>
/// Health check endpoint mappings.
/// </summary>
internal static class Endpoint
{
    /// <summary>
    /// Maps health endpoints under the provided route group.
    /// </summary>
    /// <param name="group">The route group.</param>
    /// <returns>The route group builder.</returns>
    public static RouteGroupBuilder MapHealthEndpoints(this RouteGroupBuilder group)
    {
        ArgumentNullException.ThrowIfNull(group);

        _ = group.MapGet("/", () => Results.Ok(new { Status = "Healthy" }))
            .WithName("GetHealth");
        return group;
    }
}
