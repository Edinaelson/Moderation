using System.Text.Json;
using Moderation.Interfaces;

namespace Moderation.Services;

public class GeminiModerationService : IModerationClient
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    
    public GeminiModerationService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["Gemini:GOOGLE_API_KEY"] 
                  ?? throw new Exception("API key não configurada!");
    }

    public async Task<bool> IsIllicitOrViolentAsync(string text)
    {
        var url =
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text =
                                $"Classifique o seguinte texto apenas como true (se houver violência/ilicitude) ou false (se não houver): {text}"
                        }
                    }
                }
            }
        };

        var response = await _http.PostAsJsonAsync(url, body);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var answer = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();
        
        return answer?.Trim().ToLower().Contains("true") ?? false;
    }
    
}