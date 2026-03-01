using Microsoft.AspNetCore.Mvc;
using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;

namespace CocinaManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TurnosController : ControllerBase
{
    private readonly ITurnoService _service;

    public TurnosController(ITurnoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("personal/{personalId:guid}")]
    public async Task<IActionResult> GetByPersonal(Guid personalId)
        => Ok(await _service.GetByPersonalIdAsync(personalId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTurnoDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return Ok(created);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}