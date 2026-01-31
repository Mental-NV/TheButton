namespace TheButton.Api.Features.V3.Counter;

/// <summary>
/// Response for counter queries and increments.
/// </summary>
/// <param name="Value">The global counter value.</param>
/// <param name="UserValue">The user-specific counter value, if available.</param>
public sealed record CounterResponse(long Value, long? UserValue);
