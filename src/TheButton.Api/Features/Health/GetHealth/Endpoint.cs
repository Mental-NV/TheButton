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
        _ = group.MapGet("/live", () => Results.Ok(new { Status = "Healthy" }))
            .WithName("GetLiveHealth");

        _ = group.MapHealthChecks("/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = registration => registration.Tags.Contains("ready", StringComparer.OrdinalIgnoreCase),
        }).WithName("GetReadyHealth");

        return group;
    }
}
