using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CocinaManager.API.Pages.Incidentes;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IIncidenteService _service;

    public List<IncidenteDto> Incidentes { get; set; } = new();
    public int TotalAbiertos { get; set; }
    public int TotalResueltos { get; set; }

    [BindProperty] public CreateIncidenteDto NuevoIncidente { get; set; } = new();
    [BindProperty] public string? ObservacionesResolucion { get; set; }

    public IndexModel(IIncidenteService service)
    {
        _service = service;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Incidentes";
        Incidentes = await _service.GetAllAsync();
        TotalAbiertos = Incidentes.Count(i => i.Estado == EstadoIncidente.Abierto);
        TotalResueltos = Incidentes.Count(i => i.Estado == EstadoIncidente.Resuelto);
    }

    public async Task<IActionResult> OnPostCrearAsync()
    {
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevoIncidente"))
            .ToList();
        foreach (var key in keysToRemove) ModelState.Remove(key);

        if (!ModelState.IsValid) { await OnGetAsync(); return Page(); }

        var usuario = User.Identity?.Name ?? "Sistema";
        await _service.CreateAsync(NuevoIncidente, usuario);
        TempData["Mensaje"] = "Incidente registrado correctamente.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostResolverAsync(Guid id, string? observaciones)
    {
        await _service.ResolverAsync(id, new ResolverIncidenteDto { Observaciones = observaciones });
        TempData["Mensaje"] = "Incidente marcado como resuelto.";
        return RedirectToPage();
    }
}