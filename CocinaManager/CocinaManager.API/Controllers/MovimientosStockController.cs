using Microsoft.AspNetCore.Mvc;
using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;

namespace CocinaManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientosStockController : ControllerBase
{
    private readonly IMovimientoStockService _service;

    public MovimientosStockController(IMovimientoStockService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("producto/{productoId:guid}")]
    public async Task<IActionResult> GetByProducto(Guid productoId)
        => Ok(await _service.GetByProductoIdAsync(productoId));

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] CreateMovimientoStockDto dto)
    {
        var result = await _service.RegistrarMovimientoAsync(dto);
        return Ok(result);
    }
}