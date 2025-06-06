using System.Text.Json.Serialization;

namespace OpenRouterClientNet9.Models
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