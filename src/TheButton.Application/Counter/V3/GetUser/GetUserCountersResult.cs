namespace TheButton.Application.Counter.V3.GetUser;

/// <summary>
/// Represents global and user-specific counter values.
/// </summary>
/// <param name="GlobalValue">The current global counter value.</param>
/// <param name="UserValue">The current user counter value.</param>
public record GetUserCountersResult(long GlobalValue, long UserValue);
