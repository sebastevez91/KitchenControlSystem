using Microsoft.AspNetCore.Mvc;
using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;

namespace CocinaManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HerramientasController : ControllerBase
{
    private readonly IHerramientaService _service;

    public HerramientasController(IHerramientaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("estado/{estado}")]
    public async Task<IActionResult> GetByEstado(EstadoHerramienta estado)
        => Ok(await _service.GetByEstadoAsync(estado));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHerramientaDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:guid}/estado")]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] EstadoHerramienta nuevoEstado)
    {
        var ok = await _service.CambiarEstadoAsync(id, nuevoEstado);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}