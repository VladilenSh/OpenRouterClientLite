using System.Text.Json.Serialization;

namespace OpenRouterClientLite.Models
{
    /// <summary>
    /// Запрос к API чата
    /// </summary>
    public record ChatRequest(
        [property: JsonPropertyName("model")] string Model,
        [property: JsonPropertyName("messages")] GeneratedMessage[] Messages,
        [property: JsonPropertyName("temperature")] double? Temperature = null,
        [property: JsonPropertyName("max_tokens")] int? MaxTokens = null);
}