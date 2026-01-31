using System.Text.Json.Serialization;

namespace TheButton.Mobile.Core;

/// <summary>
/// Represents the API response for a button increment.
/// </summary>
/// <param name="Value">The current counter value.</param>
public record ButtonResponse([property: JsonPropertyName("value")] int Value);
