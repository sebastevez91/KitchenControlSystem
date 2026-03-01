using Microsoft.AspNetCore.Mvc;
using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;

namespace CocinaManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenesMantenimientoController : ControllerBase
{
    private readonly IOrdenMantenimientoService _service;

    public OrdenesMantenimientoController(IOrdenMantenimientoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("herramienta/{herramientaId:guid}")]
    public async Task<IActionResult> GetByHerramienta(Guid herramientaId)
        => Ok(await _service.GetByHerramientaIdAsync(herramientaId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrdenMantenimientoDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return Ok(created);
    }

    [HttpPatch("{id:guid}/resolver")]
    public async Task<IActionResult> Resolver(Guid id, [FromBody] ResolverOrdenDto dto)
    {
        var ok = await _service.ResolverAsync(id, dto);
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("{id:guid}/cancelar")]
    public async Task<IActionResult> Cancelar(Guid id, [FromBody] ResolverOrdenDto dto)
    {
        var ok = await _service.CancelarAsync(id, dto);
        return ok ? NoContent() : NotFound();
    }
}