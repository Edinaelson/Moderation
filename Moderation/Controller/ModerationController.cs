using Microsoft.AspNetCore.Mvc;
using Moderation.DTO;
using Moderation.Interfaces;

namespace Moderation.Controller;

[ApiController]
[Route("api/[controller]")]
public class ModerationController : ControllerBase
{
    private readonly IModerationClient _gemini;

    public ModerationController(IModerationClient gemini)
    {
        _gemini = gemini;
    }

    [HttpPost]
    public async Task<ActionResult<ModerationResultDto>> Check([FromBody] string text)
    {
        var result = await _gemini.IsIllicitOrViolentAsync(text);
        
        var resultDto = new ModerationResultDto(
            IllicitOrViolence: result,
            Text: text
        );
        
        return Ok(resultDto);
    }
    
}