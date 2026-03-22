using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;

public class OllamaService
{
    private readonly HttpClient _http;
    
    public OllamaService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> Ask(string prompt)
    {
        var response = await _http.PostAsJsonAsync(
            "/api/generate",
            new
            {
                model = "gemma:2b",
                prompt = prompt,
                stream = false
            });
        
        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();

        return result?.Response ?? "No answer from ai agent";
    }

}