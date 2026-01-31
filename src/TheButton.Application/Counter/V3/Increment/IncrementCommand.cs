namespace TheButton.Application.Counter.V3.Increment;

/// <summary>
/// Command to increment the counter, optionally for a specific user.
/// </summary>
/// <param name="IdempotencyKey">Unique key for idempotency enforcement.</param>
/// <param name="UserId">Optional user identifier.</param>
public record IncrementCommand(string IdempotencyKey, Guid? UserId = null);
