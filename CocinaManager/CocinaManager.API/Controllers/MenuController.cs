using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CocinaManager.API.Controllers;

[ApiController]
[Route("api/menu")]
public class MenuController : ControllerBase
{
    private readonly IPlanMenuService _planMenuService;
    private readonly IMenuSemanalService _menuSemanalService;
    private readonly IComensalesService _comensalesService;
    private readonly IRecetaRepository _recetaRepo;
    private readonly IMenuPdfService _pdfService;
    private readonly IWebHostEnvironment _env;

    public MenuController(
        IPlanMenuService planMenuService,
        IMenuSemanalService menuSemanalService,
        IComensalesService comensalesService,
        IRecetaRepository recetaRepo,
        IMenuPdfService pdfService,
        IWebHostEnvironment env)
    {
        _planMenuService = planMenuService;
        _menuSemanalService = menuSemanalService;
        _comensalesService = comensalesService;
        _recetaRepo = recetaRepo;
        _pdfService = pdfService;
        _env = env;
    }

    [HttpGet("semana")]
    public async Task<ActionResult<MenuSemanalDto>> GetSemanaActual()
        => Ok(await _menuSemanalService.GetSemanaActualAsync());

    [HttpGet("semana/pdf")]
    public async Task<IActionResult> DescargarPdf()
    {
        var menu = await _menuSemanalService.GetSemanaActualAsync();

        byte[]? logoBytes = null;
        try
        {
            var logoPath = Path.Combine(_env.WebRootPath, "img", "logo-chacabuco.png");
            if (System.IO.File.Exists(logoPath))
                logoBytes = System.IO.File.ReadAllBytes(logoPath);
        }
        catch
        {
            logoBytes = null;
        }

        var bytes = _pdfService.GenerarPdfSemanal(menu, logoBytes);
        return File(bytes, "application/pdf", $"menu-semanal-{DateTime.Now:yyyyMMdd}.pdf");
    }

    [HttpGet]
    public async Task<ActionResult<List<PlanMenuDto>>> GetAll()
        => Ok(await _planMenuService.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PlanMenuDto>> GetById(Guid id)
    {
        var plan = await _planMenuService.GetByIdAsync(id);
        return plan == null ? NotFound() : Ok(plan);
    }

    [HttpPost]
    public async Task<ActionResult<PlanMenuDto>> Create(CreatePlanMenuDto dto)
    {
        var creado = await _planMenuService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => await _planMenuService.DeleteAsync(id) ? NoContent() : NotFound();

    [HttpDelete("{planId:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid planId, Guid itemId)
        => await _planMenuService.RemoveItemAsync(planId, itemId) ? NoContent() : NotFound();

    [HttpPost("comensales")]
    public async Task<IActionResult> RegistrarComensales(RegistrarComensalesDto dto)
    {
        await _comensalesService.RegistrarAsync(dto.Fecha, dto.Cantidad);
        return NoContent();
    }

    // Recetas disponibles para asignar al plan semanal. En un mundo ideal esto vendría de un IRecetaService, pero por ahora lo resolvemos directo desde el contexto.
    [HttpGet("recetas-disponibles")]
    public async Task<ActionResult<List<RecetaSimpleDto>>> GetRecetasDisponibles()
    {
        var recetas = await _recetaRepo.GetAllSimpleAsync();
        return Ok(recetas);
    }

    [HttpPut("celda")]
    public async Task<IActionResult> SetReceta([FromBody] SetRecetaCeldaDto dto)
    {
        await _planMenuService.SetRecetaAsync(dto.DiaSemana, dto.TipoMenu, dto.RecetaId, dto.Rol);
        return NoContent();
    }

    [HttpDelete("celda")]
    public async Task<IActionResult> LimpiarCelda([FromQuery] DiaSemana diaSemana, [FromQuery] TipoMenu tipoMenu, [FromQuery] RolPlato? rol)
    {
        await _planMenuService.LimpiarCeldaAsync(diaSemana, tipoMenu, rol);
        return NoContent();
    }
}
