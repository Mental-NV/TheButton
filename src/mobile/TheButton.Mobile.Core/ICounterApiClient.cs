namespace TheButton.Mobile.Core;

/// <summary>
/// Defines counter API operations for the mobile client.
/// </summary>
public interface ICounterApiClient
{
    /// <summary>
    /// Increments the counter.
    /// </summary>
    /// <returns>The new counter value.</returns>
    Task<int> IncrementAsync();

    /// <summary>
    /// Gets the current counter value.
    /// </summary>
    /// <returns>The current counter value.</returns>
    Task<int> GetAsync();
}
