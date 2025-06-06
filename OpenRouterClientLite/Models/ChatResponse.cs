using OpenRouterClientNet9.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenRouterClientNet9.Models
{
    internal class ChatResponse
    {
    }
}
namespace OpenRouterClientNet9
{
    /// <summary>
    /// Ответ от API OpenRouter, чат((Тело сообщения, причина остановки),(токены на промт, токены на ответ, токены в сумме))
    /// </summary>
    public record ChatResponse(
        [property: JsonPropertyName("choices")] ChatCompletionChoice[] ChatCompletionChoice,
        [property: JsonPropertyName("usage")] UsageTokens UsageTokens);

    /// <summary>
    /// Вариант ответа модели(Тело сообщения, причина остановки)
    /// </summary>
    public record ChatCompletionChoice(
        [property: JsonPropertyName("message")] GeneratedMessage GeneratedMessage,
        [property: JsonPropertyName("finish_reason")] string FinishReason);

    /// <summary>
    /// Использованные токены(токены на промт, токены на ответ, токены в сумме)
    /// </summary>
    public record UsageTokens(
        [property: JsonPropertyName("prompt_tokens")] int PromptTokens,
        [property: JsonPropertyName("completion_tokens")] int CompletionTokens,
        [property: JsonPropertyName("total_tokens")] int TotalTokens);
}