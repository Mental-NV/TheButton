namespace TheButton.Application.Abstractions;

/// <summary>
/// Abstraction for writing counter increments with idempotency.
/// </summary>
public interface ICounterWriter
{
    /// <summary>
    /// Increments the global counter atomically.
    /// </summary>
    /// <param name="idempotencyKey">Unique key for idempotency enforcement.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result containing the global value.</returns>
    Task<GlobalIncrementResult> IncrementGlobalAsync(
        string idempotencyKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Increments both the global counter and the per-user counter atomically.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="idempotencyKey">Unique key for idempotency enforcement.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result containing the global value and user value.</returns>
    Task<UserIncrementResult> IncrementUserAsync(
        Guid userId,
        string idempotencyKey,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of a global increment operation.
/// </summary>
/// <param name="GlobalValue">The monotonic global ordering number from write.Events.Position.</param>
public record GlobalIncrementResult(long GlobalValue);

/// <summary>
/// Result of a user increment operation.
/// </summary>
/// <param name="GlobalValue">The monotonic global ordering number from write.Events.Position.</param>
/// <param name="UserValue">The per-user counter value from read.UserCounters.Value.</param>
public record UserIncrementResult(long GlobalValue, long UserValue);
