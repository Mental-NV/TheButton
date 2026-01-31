namespace TheButton.Domain.Features.V3.Counter;

/// <summary>
/// Represents the result of a counter increment with user-specific data.
/// </summary>
/// <param name="Value">The current global counter value.</param>
/// <param name="UserValue">The current user counter value, if available.</param>
public record IncrementResult(long Value, long? UserValue);
