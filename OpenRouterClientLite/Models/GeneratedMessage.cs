
using System.Text.Json.Serialization;

namespace OpenRouterClientLite.Models
{
    /// <summary>
    /// Сообщение в чате (роль + контент)
    /// </summary>
    public record GeneratedMessage(
        [property: JsonPropertyName("role")] string Role,
        [property: JsonPropertyName("content")] string Content);
}
