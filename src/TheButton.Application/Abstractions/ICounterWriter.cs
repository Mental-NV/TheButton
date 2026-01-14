namespace TheButton.Application.Abstractions;

/// <summary>
/// Abstraction for writing counter increments with idempotency.
/// </summary>
public interface ICounterWriter
{
    /// <summary>
    /// Increments the counter atomically, optionally scoped to a user.
    /// </summary>
    /// <param name="idempotencyKey">Unique key for idempotency enforcement.</param>
    /// <param name="userId">Optional user identifier. If provided, increments also the user counter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result containing the global value and optional user value.</returns>
    Task<IncrementResult> IncrementAsync(
        string idempotencyKey,
        Guid? userId = null,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of an increment operation.
/// </summary>
/// <param name="GlobalValue">The monotonic global ordering number from write.Events.Position.</param>
/// <param name="UserValue">The per-user counter value (UserVersion), if userId was provided.</param>
public record IncrementResult(long GlobalValue, long? UserValue);
