using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moderation.Data;
using Moderation.DTO;
using Moderation.Interfaces;
using Moderation.Model;
using Swashbuckle.AspNetCore.Annotations;

namespace Moderation.Controller;

[ApiController]
[Route("api/[controller]")]

[SwaggerTag("Aqui é um sistema de moderação de texto. Use os endpoints abaixo para filtrar logs.")]
public class ModerationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IModerationClient _gemini;

    public ModerationController(IModerationClient gemini, ApplicationDbContext context)
    {
        _gemini = gemini;
        _context = context;
    }

    [HttpPost]
    [SwaggerOperation(
        "Informe um texto para ser analisado", 
        "Ex: Ele quer casar, isso retorna false, Ele deseja matar, isso retorna true")]
    public async Task<ActionResult<ModerationResultDto>> Check([FromBody] string text)
    {
        var result = await _gemini.IsIllicitOrViolentAsync(text);
        
        var resultDto = new ModerationResultDto(
            IllicitOrViolence: result,
            Text: text
        );
        
        var log = new ModerationLog
        {
            Text = text,
            IsIllicitOrViolent = result,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.ModerationLogs.Add(log);
        await _context.SaveChangesAsync();
        
        return Ok(resultDto);
    }

    [HttpGet("true")]
    [SwaggerOperation("Retorne todos casos deferido como verdade")]
    public async Task<ActionResult<IEnumerable<ModerationLog>>> GetTrueLogs()
    {
        var logs = await _context.ModerationLogs
            .Where(x => x.IsIllicitOrViolent == true)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
        
        return Ok(logs);
    }

    [HttpGet("false")]
    [SwaggerOperation("Retorne todos casos deferido como falso")]
    public async Task<ActionResult<IEnumerable<ModerationLog>>> GetFalseLogs()
    {
        var logs = await _context.ModerationLogs
            .Where(x=> x.IsIllicitOrViolent == false)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
        
        return Ok(logs);
    }
    
    [HttpGet("byday")]
    [SwaggerOperation(
        Summary = "Filtrar logs por dia",
        Description = "Informe a data desejada no formato ISO (ex: 2025-08-27). Retorna todos os logs daquele dia."
    )]
    public async Task<ActionResult<IEnumerable<object>>> GetByDay(
        [FromQuery, SwaggerParameter("Data no formato ISO: 2025-08-27", Required = true)]
        DateTime date)
    {
        var start = date.Date; // 00:00:00
        var end = date.Date.AddDays(1).AddTicks(-1); // 23:59:59.9999999

        var logs = await _context.ModerationLogs
            .Where(x => x.CreatedAt >= start && x.CreatedAt <= end)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new 
            {
                x.CreatedAt,
                x.IsIllicitOrViolent,
                x.Text
            })
            .ToListAsync();

        return Ok(logs);
    }

    [HttpGet("raw-count")]
    [SwaggerOperation("Consultar quantos registros tem", " Retorna o total de registros")]
    public async Task<IActionResult> GetRawCount()
    {
        await using var conn = _context.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync();
        
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM ModerationLogs";

        var result = await cmd.ExecuteScalarAsync();
        var count = Convert.ToInt64(result); 

        return Ok(new { total = count });
    }
    
}