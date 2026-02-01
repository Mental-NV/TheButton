using Microsoft.AspNetCore.Mvc;
using TheButton.Application.Abstractions;
using TheButton.Application.Counter.V3.GetGlobal;
using TheButton.Application.Counter.V3.GetUser;
using TheButton.Application.Counter.V3.Increment;
using TheButton.Domain.Features.V3.Counter;

namespace TheButton.Api.Features.V3.Counter;

/// <summary>
/// Endpoint mappings for v3 counter operations.
/// </summary>
internal static class CounterEndpoints
{
    /// <summary>
    /// Maps v3 counter endpoints under the provided route group.
    /// </summary>
    /// <param name="group">The route group.</param>
    /// <returns>The route group builder.</returns>
    public static RouteGroupBuilder MapV3CounterEndpoints(this RouteGroupBuilder group)
    {
        // POST /api/v3/counter
        _ = group.MapPost("/", HandleGlobalIncrementAsync);

        // POST /api/v3/counter/{userId}
        _ = group.MapPost("/{userId:guid}", HandleUserIncrementAsync);

        // GET /api/v3/counter
        _ = group.MapGet("/", HandleGlobalReadAsync);

        // GET /api/v3/counter/{userId}
        _ = group.MapGet("/{userId:guid}", HandleUserReadAsync);

        return group;
    }

    /// <summary>
    /// Handles the global increment command.
    /// </summary>
    /// <param name="idempotencyKey">The optional idempotency key.</param>
    /// <param name="handler">The increment handler.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The counter response.</returns>
    internal static async Task<
        Microsoft.AspNetCore.Http.HttpResults.Results<
            Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse>,
            Microsoft.AspNetCore.Http.HttpResults.Conflict>>
        HandleGlobalIncrementAsync(
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
        [FromServices] IncrementHandler handler,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(handler);

        try
        {
            IncrementCommand command = IncrementCommandFactory.Create(idempotencyKey: idempotencyKey, userId: null);
            IncrementResult result = await handler.Handle(command, ct).ConfigureAwait(false);
            return TypedResults.Ok(new CounterResponse(Value: result.Value, UserValue: result.UserValue));
        }
        catch (CounterWriteConflictException)
        {
            return TypedResults.Conflict();
        }
    }

    /// <summary>
    /// Handles a user-scoped increment command.
    /// </summary>
    /// <param name="idempotencyKey">The optional idempotency key.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="handler">The increment handler.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The counter response.</returns>
    internal static async Task<
        Microsoft.AspNetCore.Http.HttpResults.Results<
            Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse>,
            Microsoft.AspNetCore.Http.HttpResults.Conflict>>
        HandleUserIncrementAsync(
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
        Guid userId,
        [FromServices] IncrementHandler handler,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(handler);

        try
        {
            IncrementCommand command = IncrementCommandFactory.Create(idempotencyKey: idempotencyKey, userId: userId);
            IncrementResult result = await handler.Handle(command, ct).ConfigureAwait(false);
            return TypedResults.Ok(new CounterResponse(Value: result.Value, UserValue: result.UserValue));
        }
        catch (CounterWriteConflictException)
        {
            return TypedResults.Conflict();
        }
    }

    /// <summary>
    /// Handles the global counter read query.
    /// </summary>
    /// <param name="handler">The query handler.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The counter response.</returns>
    internal static async Task<Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse>> HandleGlobalReadAsync(
        [FromServices] GetGlobalQueryHandler handler,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(handler);

        GetGlobalResult result = await handler.Handle(new GetGlobalQuery(), ct).ConfigureAwait(false);
        return TypedResults.Ok(new CounterResponse(Value: result.Value, UserValue: null));
    }

    /// <summary>
    /// Handles the user counter read query.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="handler">The query handler.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The counter response.</returns>
    internal static async Task<Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse>> HandleUserReadAsync(
        Guid userId,
        [FromServices] GetUserCountersQueryHandler handler,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(handler);

        GetUserCountersResult result = await handler
            .Handle(new GetUserCountersQuery(userId), ct)
            .ConfigureAwait(false);

        return TypedResults.Ok(new CounterResponse(Value: result.GlobalValue, UserValue: result.UserValue));
    }
}
