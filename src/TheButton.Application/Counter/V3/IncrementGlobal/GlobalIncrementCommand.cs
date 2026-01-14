namespace TheButton.Application.Counter.V3.IncrementGlobal;

/// <summary>
/// Command to increment the global counter.
/// </summary>
/// <param name="IdempotencyKey">Unique key for idempotency enforcement.</param>
public record GlobalIncrementCommand(string IdempotencyKey);
