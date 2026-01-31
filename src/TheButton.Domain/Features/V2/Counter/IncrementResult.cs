namespace TheButton.Domain.Features.V2.Counter;

/// <summary>
/// Represents the result of a counter increment.
/// </summary>
/// <param name="Value">The current global counter value.</param>
public record IncrementResult(long Value);
