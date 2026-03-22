using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("chat")]
public class ChatController : ControllerBase
{
    private readonly OllamaService _ollama;

    public ChatController(OllamaService ollama)
    {
        _ollama = ollama;
    }

    [HttpPost]
    public async Task<IActionResult> Post ([FromBody] OllamaRequest request)
    {
        var response = await _ollama.Ask(request.Prompt);

        return Ok(response);
    }
}