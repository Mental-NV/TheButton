namespace TheButton.Api.Features.V2.Counter;

/// <summary>
/// Response for v2 counter increments.
/// </summary>
/// <param name="Value">The global counter value.</param>
public sealed record CounterResponse(long Value);
