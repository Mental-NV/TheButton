using TheButton.Application.Counter.V2.Increment;
using TheButton.Domain.Features.V2.Counter;

namespace TheButton.Api.Features.V2.Counter;

/// <summary>
/// Endpoint mappings for v2 counter operations.
/// </summary>
internal static class CounterEndpoints
{
    /// <summary>
    /// Maps v2 counter endpoints under the provided route group.
    /// </summary>
    /// <param name="group">The route group.</param>
    /// <returns>The route group builder.</returns>
    public static RouteGroupBuilder MapV2CounterEndpoints(this RouteGroupBuilder group)
    {
        _ = group.MapPost("/", Increment);
        return group;
    }

    /// <summary>
    /// Handles the v2 increment command.
    /// </summary>
    /// <param name="handler">The increment handler.</param>
    /// <returns>The counter response.</returns>
    public static Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse> Increment(IncrementHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        IncrementResult result = handler.Handle(new IncrementCommand());
        return TypedResults.Ok(new CounterResponse(result.Value));
    }
}
