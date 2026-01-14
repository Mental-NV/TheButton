namespace TheButton.Application.Abstractions;

/// <summary>
/// Abstraction for reading counter values.
/// </summary>
public interface ICounterReadRepository
{
    /// <summary>
    /// Gets the global counter value.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The global counter value.</returns>
    Task<long> GetGlobalValueAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the per-user counter value.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user counter value, or 0 if the user has no recorded counter.</returns>
    Task<long> GetUserValueAsync(Guid userId, CancellationToken cancellationToken = default);
}
