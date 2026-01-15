using TheButton.Domain.Features.V3.Counter;

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

