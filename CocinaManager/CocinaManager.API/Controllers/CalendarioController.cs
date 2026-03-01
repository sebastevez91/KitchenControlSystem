using CocinaManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CocinaManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CalendarioController : ControllerBase
{
    private readonly ITurnoService _turnoService;

    public CalendarioController(ITurnoService turnoService)
    {
        _turnoService = turnoService;
    }

    [HttpGet("turnos")]
    public async Task<IActionResult> GetTurnos([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var turnos = await _turnoService.GetByRangoAsync(start, end);

        var events = turnos.Select(t => new
        {
            id = t.Id,
            title = $"{t.NombrePersonal} ({t.TipoTurno})",
            start = t.Fecha.ToString("yyyy-MM-dd"),
            color = t.TipoTurno.ToString() switch
            {
                "Manana" => "#f7971e",
                "Tarde" => "#4facfe",
                "Noche" => "#1a1a2e",
                _ => "#667eea"
            },
            extendedProps = new
            {
                personalId = t.PersonalId,
                nombrePersonal = t.NombrePersonal,
                tipoTurno = t.TipoTurno.ToString()
            }
        });

        return Ok(events);
    }
}