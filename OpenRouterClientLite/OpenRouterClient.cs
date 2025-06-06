using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenRouterClientNet9.Models;

namespace OpenRouterClientNet9
{
    /// <summary>
    /// Клиент для работы с API OpenRouter
    /// </summary>
    public class OpenRouterClient : IDisposable
    {
        // HTTP-клиент для выполнения запросов к API
        private readonly HttpClient _httpClient;

        // Настройки сериализации JSON
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Конструктор клиента OpenRouter API
        /// </summary>
        /// <param name="apiKey">API ключ для аутентификации</param>
        /// <param name="appName">Название приложения (необязательно)</param>
        /// <param name="siteUrl">URL сайта (необязательно)</param>
        public OpenRouterClient(string apiKey, string? appName = null, string? siteUrl = null)
        {
            // Инициализация HTTP-клиента с базовым адресом и заголовком авторизации
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://openrouter.ai/api/v1/"),
                DefaultRequestHeaders = { { "Authorization", $"Bearer {apiKey}" } }
            };

            // Добавление необязательных заголовков, если они указаны
            if (!string.IsNullOrEmpty(appName))
                _httpClient.DefaultRequestHeaders.Add("X-Title", appName);

            if (!string.IsNullOrEmpty(siteUrl))
                _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", siteUrl);

            // Настройка параметров сериализации JSON:
            // - имена свойств в snake_case
            // - игнорирование null значений при сериализации
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        /// <summary>
        /// Получает список доступных моделей
        /// </summary>
        /// <param name="cts">Токен отмены</param>
        /// <returns>Массив информации о моделях</returns>
        public async Task<ModelInfo[]> GetModelsAsync(CancellationToken cts = default)
        {
            // Запрос к API для получения списка моделей
            var response = await _httpClient.GetFromJsonAsync<JsonDocument>("models", cts)
                ?? throw new InvalidOperationException("API returned null response.");

            // Извлечение массива моделей из свойства "data" ответа
            var models = response.RootElement.GetProperty("data");

            // Десериализация массива моделей
            return models.Deserialize<ModelInfo[]>(_jsonOptions)
                ?? throw new InvalidOperationException("Failed to deserialize models list.");
        }

        /// <summary>
        /// Генерирует ответ на основе промта с использованием указанной модели
        /// </summary>
        /// <param name="model">Идентификатор модели</param>
        /// <param name="prompt">Текст промта</param>
        /// <param name="temperature">Температура генерации (необязательно)</param>
        /// <param name="maxTokens">Максимальное количество токенов (необязательно)</param>
        /// <param name="cts">Токен отмены</param>
        /// <returns>Ответ модели</returns>
        public async Task<ChatResponse> GenerateResponseAsync(
            string model,
            string prompt,
            double? temperature = null,
            int? maxTokens = null,
            CancellationToken cts = default)
        {
            // Создание запроса с указанными параметрами
            var request = new ChatRequest(model, [new GeneratedMessage("user", prompt)], temperature, maxTokens);

            // Отправка POST-запроса к API
            using var response = await _httpClient.PostAsJsonAsync(
                "chat/completions", request, _jsonOptions, cts);

            // Проверка успешности запроса
            response.EnsureSuccessStatusCode();

            // Чтение и десериализация ответа
            return await response.Content.ReadFromJsonAsync<ChatResponse>(_jsonOptions, cts)
                ?? throw new InvalidOperationException("API returned null response.");
        }

        /// <summary>
        /// Освобождает ресурсы HTTP-клиента
        /// </summary>
        public void Dispose() => _httpClient.Dispose();
    }
}