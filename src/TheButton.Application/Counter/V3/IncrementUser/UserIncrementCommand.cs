namespace TheButton.Application.Counter.V3.IncrementUser;

/// <summary>
/// Command to increment both the global counter and a per-user counter.
/// </summary>
/// <param name="UserId">User identifier.</param>
/// <param name="IdempotencyKey">Unique key for idempotency enforcement.</param>
public record UserIncrementCommand(Guid UserId, string IdempotencyKey);
