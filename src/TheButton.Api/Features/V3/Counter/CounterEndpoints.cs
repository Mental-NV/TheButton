using Microsoft.AspNetCore.Mvc;
using TheButton.Application.Counter.V3.Increment;

namespace TheButton.Api.Features.V3.Counter;

public static class CounterEndpoints
{
    public static RouteGroupBuilder MapV3CounterEndpoints(this RouteGroupBuilder group)
    {
        // POST /api/v3/counter
        // Supports optional userId via query parameter: POST /api/v3/counter?userId=...
        group.MapPost("/", async (
            [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
            [FromQuery] Guid? userId,
            [FromServices] IncrementHandler handler,
            CancellationToken ct) =>
        {
            var command = new IncrementCommand(idempotencyKey, userId);
            var result = await handler.Handle(command, ct);
            return Results.Ok(result);
        });

        // GET /api/v3/counter
        group.MapGet("/", async (
             [FromServices] TheButton.Application.Abstractions.ICounterReadRepository repository,
             CancellationToken ct) =>
        {
            var globalValue = await repository.GetGlobalValueAsync(ct);
            return Results.Ok(new { globalValue });
        });

        return group;
    }
}
