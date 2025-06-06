using System.Text.Json.Serialization;

namespace OpenRouterClientNet9.Models
{
    /// <summary>
    /// Информация о модели
    /// </summary>
    public record ModelInfo(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("pricing")] PricingInfo Pricing);

    /// <summary>
    /// Цены за использование модели
    /// </summary>
    public record PricingInfo(
        [property: JsonPropertyName("prompt")] string? Prompt,
        [property: JsonPropertyName("completion")] string? Completion);
}