using Microsoft.AspNetCore.Mvc;
using TheButton.Application.Counter.V3.Increment;
using TheButton.Application.Counter.V3.GetGlobal;
using TheButton.Application.Counter.V3.GetUser;
using TheButton.Domain.Features.V3.Counter;

namespace TheButton.Api.Features.V3.Counter;

public static class CounterEndpoints
{
    public static RouteGroupBuilder MapV3CounterEndpoints(this RouteGroupBuilder group)
    {
        // POST /api/v3/counter
        group.MapPost("/", HandleGlobalIncrementAsync);

        // POST /api/v3/counter/{userId}
        group.MapPost("/{userId:guid}", HandleUserIncrementAsync);

        // GET /api/v3/counter
        group.MapGet("/", HandleGlobalReadAsync);

        // GET /api/v3/counter/{userId}
        group.MapGet("/{userId:guid}", HandleUserReadAsync);

        return group;
    }

    internal static async Task<Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse>> HandleGlobalIncrementAsync(
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
        [FromServices] IncrementHandler handler,
        CancellationToken ct)
    {
        var command = IncrementCommandFactory.Create(idempotencyKey, null);
        var result = await handler.Handle(command, ct);
        return TypedResults.Ok(new CounterResponse(result.Value, result.UserValue));
    }

    internal static async Task<Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse>> HandleUserIncrementAsync(
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
        Guid userId,
        [FromServices] IncrementHandler handler,
        CancellationToken ct)
    {
        var command = IncrementCommandFactory.Create(idempotencyKey, userId);
        var result = await handler.Handle(command, ct);
        return TypedResults.Ok(new CounterResponse(result.Value, result.UserValue));
    }

    internal static async Task<Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse>> HandleGlobalReadAsync(
        [FromServices] GetGlobalQueryHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetGlobalQuery(), ct);
        return TypedResults.Ok(new CounterResponse(result.Value, null));
    }

    internal static async Task<Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse>> HandleUserReadAsync(
        Guid userId,
        [FromServices] GetUserCountersQueryHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetUserCountersQuery(userId), ct);

        return TypedResults.Ok(new CounterResponse(result.GlobalValue, result.UserValue));
    }
}
