using Microsoft.AspNetCore.Mvc;
using TheButton.Application.Counter.V3.Increment;
using TheButton.Domain.Features.V3.Counter;

namespace TheButton.Api.Features.V3.Counter;

public static class CounterEndpoints
{
    public static RouteGroupBuilder MapV3CounterEndpoints(this RouteGroupBuilder group)
    {
        // POST /api/v3/counter
        group.MapPost("/", async (
            [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
            [FromServices] IncrementHandler handler,
            CancellationToken ct) =>
        {
            idempotencyKey = string.IsNullOrWhiteSpace(idempotencyKey) ? Guid.NewGuid().ToString() : idempotencyKey;
            var command = new IncrementCommand(idempotencyKey, null);
            var result = await handler.Handle(command, ct);
            return Results.Ok(new CounterResponse(result.Value, result.UserValue));
        });

        // POST /api/v3/counter/{userId}
        group.MapPost("/{userId:guid}", async (
            [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
            Guid userId,
            [FromServices] IncrementHandler handler,
            CancellationToken ct) =>
        {
            idempotencyKey = string.IsNullOrWhiteSpace(idempotencyKey) ? Guid.NewGuid().ToString() : idempotencyKey;
            var command = new IncrementCommand(idempotencyKey, userId);
            var result = await handler.Handle(command, ct);
            return Results.Ok(new CounterResponse(result.Value, result.UserValue));
        });

        // GET /api/v3/counter
        group.MapGet("/", async (
             [FromServices] TheButton.Application.Abstractions.ICounterReadRepository repository,
             CancellationToken ct) =>
        {
            var globalValue = await repository.GetGlobalValueAsync(ct);
            return Results.Ok(new CounterResponse(globalValue, null));
        });

        // GET /api/v3/counter/{userId}
        group.MapGet("/{userId:guid}", async (
            Guid userId,
            [FromServices] TheButton.Application.Abstractions.ICounterReadRepository repository,
            CancellationToken ct) =>
        {
            var globalValue = await repository.GetGlobalValueAsync(ct);
            var userValue = await repository.GetUserValueAsync(userId, ct);

            return Results.Ok(new CounterResponse(globalValue, userValue));
        });

        return group;
    }
}
