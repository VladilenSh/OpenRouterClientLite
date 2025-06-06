# OpenRouterClientLite

[![NuGet](https://img.shields.io/nuget/v/OpenRouterClientLite)](https://www.nuget.org/packages/OpenRouterClientLite/)
![Static Badge](https://img.shields.io/badge/LicenseGPL-3.0-only)


Легковесный .NET клиент для интеграции с [OpenRouter API](https://openrouter.ai/). Поддерживает все основные функции API, включая получение списка моделей и генерацию текста.

## Особенности

- Получение информации о доступных моделях с ценами
- Генерация текстовых ответов через чат-интерфейс
- Поддержка параметров генерации (температура, макс. токены)
- Автоматическая сериализация/десериализация запросов
- Поддержка отмены операций (CancellationToken)
- Правильная обработка HTTP-заголовков (аутентификация, реферер)
- Полная типизация моделей данных.

### Установка

Добавьте пакет через NuGet:

bash
Install-Package OpenRouterClientLite
Или через .NET CLI:

bash
dotnet add package OpenRouterClientLite


###  Быстрый старт


Инициализация клиента
```csharp
using OpenRouterClientLite;

var apiKey = "your_api_key_here";
var client = new OpenRouterClient(
    apiKey: apiKey,
    appName: "MyAwesomeApp",    // Необязательно
    siteUrl: "https://myapp.com" // Необязательно
);
```

### Получение списка моделей


```csharp
try
{
    var models = await client.GetModelsAsync();
    foreach (var model in models)
    {
        Console.WriteLine($"ID: {model.Id}");
        Console.WriteLine($"Name: {model.Name}");
        Console.WriteLine($"Description: {model.Description}");
        Console.WriteLine($"Pricing: {model.Pricing.Prompt} (prompt) / {model.Pricing.Completion} (completion)");
        Console.WriteLine();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### Генерация текста

```csharp
try
{
    var response = await client.GenerateResponseAsync(
        model: "openai/gpt-3.5-turbo",
        prompt: "Расскажи интересный факт о космосе",
        temperature: 0.7,
        maxTokens: 150
    );

    var message = response.ChatCompletionChoice[0].GeneratedMessage.Content;
    var usage = response.UsageTokens;
    
    Console.WriteLine($"Response: {message}");
    Console.WriteLine($"Tokens used: {usage.PromptTokens} (prompt) + {usage.CompletionTokens} (completion) = {usage.TotalTokens}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```
### Модели данных
Запрос генерации (ChatRequest)
|Параметр|	Тип|	Описание|
|---|-----|-------|
|Model	|string|	Идентификатор модели|
|Messages	|GeneratedMessage[]	|Массив сообщений|
|Temperature|	double?|	Креативность (0-1)|
|MaxTokens|	int?	|Макс. количество токенов в ответе|

### Ответ (ChatResponse)

```csharp
public record ChatResponse(
    ChatCompletionChoice[] Choices,
    UsageTokens Usage);

public record ChatCompletionChoice(
    GeneratedMessage Message,
    string FinishReason);

public record UsageTokens(
    int PromptTokens,
    int CompletionTokens,
    int TotalTokens);
```
### Сообщение (GeneratedMessage)
```csharp
public record GeneratedMessage(
    string Role,   // "user", "assistant", "system"
    string Content);
```
### Рекомендации

API Keys: Получите ключ на OpenRouter Dashboard

Модели: Актуальный список моделей доступен через GetModelsAsync()

Обработка ошибок: Все методы выбрасывают исключения при HTTP-ошибках

### Освобождение ресурсов: Клиент реализует IDisposable
```csharp
using (var client = new OpenRouterClient(apiKey))
{
    // работа с клиентом
}
```
