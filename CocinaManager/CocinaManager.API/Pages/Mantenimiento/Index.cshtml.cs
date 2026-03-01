using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CocinaManager.API.Pages.Mantenimiento;

public class IndexModel : PageModel
{
    private readonly IHerramientaService _herramientaService;
    private readonly IOrdenMantenimientoService _ordenService;

    public List<HerramientaDto> Herramientas { get; set; } = new();
    public List<OrdenMantenimientoDto> Ordenes { get; set; } = new();

    [BindProperty] public CreateHerramientaDto NuevaHerramienta { get; set; } = new();
    [BindProperty] public CreateOrdenMantenimientoDto NuevaOrden { get; set; } = new();

    public IndexModel(IHerramientaService herramientaService, IOrdenMantenimientoService ordenService)
    {
        _herramientaService = herramientaService;
        _ordenService = ordenService;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Mantenimiento";
        Herramientas = await _herramientaService.GetAllAsync();
        Ordenes = await _ordenService.GetAllAsync();
    }

    public async Task<IActionResult> OnPostCrearHerramientaAsync()
    {
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevaHerramienta"))
            .ToList();

        foreach (var key in keysToRemove)
            ModelState.Remove(key);

        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        await _herramientaService.CreateAsync(NuevaHerramienta);
        TempData["Mensaje"] = "Herramienta registrada correctamente.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCrearOrdenAsync()
    {
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevaOrden"))
            .ToList();

        foreach (var key in keysToRemove)
            ModelState.Remove(key);

        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        await _ordenService.CreateAsync(NuevaOrden);
        TempData["Mensaje"] = "Orden de mantenimiento creada correctamente.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostResolverOrdenAsync(Guid id, string? observaciones)
    {
        await _ordenService.ResolverAsync(id, new ResolverOrdenDto { Observaciones = observaciones });
        TempData["Mensaje"] = "Orden resuelta. Herramienta devuelta a estado Operativa.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCancelarOrdenAsync(Guid id, string? observaciones)
    {
        await _ordenService.CancelarAsync(id, new ResolverOrdenDto { Observaciones = observaciones });
        TempData["Mensaje"] = "Orden cancelada.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCambiarEstadoHerramientaAsync(Guid id, EstadoHerramienta estado)
    {
        await _herramientaService.CambiarEstadoAsync(id, estado);
        TempData["Mensaje"] = "Estado de herramienta actualizado.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEliminarHerramientaAsync(Guid id)
    {
        await _herramientaService.DeleteAsync(id);
        TempData["Mensaje"] = "Herramienta eliminada.";
        return RedirectToPage();
    }
}