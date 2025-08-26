using Microsoft.AspNetCore.Mvc;
using Moderation.DTO;
using Moderation.Services;

namespace Moderation.Controller;

[ApiController]
[Route("api/[controller]")]
public class ModerationController : ControllerBase
{
    
    private readonly ModerationIA _moderationService;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    
    public ModerationController(ModerationIA moderationService)
    {
        _moderationService = moderationService;
    }
    
    [HttpPost]
    public IActionResult Moderate([FromBody] string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return BadRequest("Text is required.");
        }
        
        try
        {
            _semaphore.Wait(); // bloqueia chamadas simultâneas
            var result = _moderationService.Moderate(text);
            return Ok(result);
        }
        catch (System.ClientModel.ClientResultException ex) when (ex.Status == 429)
        {
            // Retorna mensagem amigável se ultrapassar limite
            return StatusCode(429, "Too many requests to OpenAI API. Please try again later.");
        }
        finally
        {
            _semaphore.Release();
        }
        
    }
    
    
    
}