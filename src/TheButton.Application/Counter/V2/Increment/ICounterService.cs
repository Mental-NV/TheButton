namespace TheButton.Application.Counter.V2.Increment;

/// <summary>
/// Provides counter operations for v2.
/// </summary>
public interface ICounterService
{
    /// <summary>
    /// Gets the current counter value.
    /// </summary>
    /// <returns>The current counter value.</returns>
    int GetCount();

    /// <summary>
    /// Increments the counter value.
    /// </summary>
    /// <returns>The new counter value.</returns>
    int Increment();
}
