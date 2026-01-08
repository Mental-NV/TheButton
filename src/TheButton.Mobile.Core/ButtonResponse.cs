using System.Text.Json.Serialization;

namespace TheButton.Mobile.Core;

public record ButtonResponse
(
    [property: JsonPropertyName("value")] int Value
);
