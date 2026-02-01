namespace TheButton.Api;

/// <summary>
/// Shared health check tags for the API.
/// </summary>
internal static class HealthCheckTags
{
    /// <summary>
    /// Tag used to mark readiness checks.
    /// </summary>
    public static readonly string[] Ready =
        ["ready"];
}
