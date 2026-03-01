using CocinaManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CocinaManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReportesController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    [HttpGet("personal")]
    public async Task<IActionResult> Personal()
    {
        var bytes = await _reporteService.ExportarPersonalAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Personal_{DateTime.Now:yyyyMMdd}.xlsx");
    }

    [HttpGet("turnos")]
    public async Task<IActionResult> Turnos()
    {
        var bytes = await _reporteService.ExportarTurnosAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Turnos_{DateTime.Now:yyyyMMdd}.xlsx");
    }

    [HttpGet("productos-stock")]
    public async Task<IActionResult> ProductosStock()
    {
        var bytes = await _reporteService.ExportarProductosStockAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Stock_{DateTime.Now:yyyyMMdd}.xlsx");
    }

    [HttpGet("movimientos-stock")]
    public async Task<IActionResult> MovimientosStock()
    {
        var bytes = await _reporteService.ExportarMovimientosStockAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Movimientos_{DateTime.Now:yyyyMMdd}.xlsx");
    }
}