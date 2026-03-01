using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CocinaManager.API.Pages.Ausencias;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly IAusenciaService _ausenciaService;
    private readonly IPersonalService _personalService;

    public List<AusenciaDto> Ausencias { get; set; } = new();
    public List<PersonalDto> Personal { get; set; } = new();

    // Stats
    public int TotalDiasAusencia { get; set; }
    public AusenciaDto? UltimaAusencia { get; set; }

    [BindProperty] public CreateAusenciaDto NuevaAusencia { get; set; } = new();

    public IndexModel(IAusenciaService ausenciaService, IPersonalService personalService)
    {
        _ausenciaService = ausenciaService;
        _personalService = personalService;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Ausencias";
        Ausencias = await _ausenciaService.GetAllAsync();
        Personal = await _personalService.GetAllAsync();
        TotalDiasAusencia = Ausencias.Sum(a => a.DiasAusencia);
        UltimaAusencia = Ausencias.FirstOrDefault();
    }

    public async Task<IActionResult> OnPostCrearAsync()
    {
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevaAusencia"))
            .ToList();
        foreach (var key in keysToRemove) ModelState.Remove(key);

        if (!ModelState.IsValid) { await OnGetAsync(); return Page(); }

        if (NuevaAusencia.FechaFin < NuevaAusencia.FechaInicio)
        {
            ModelState.AddModelError("NuevaAusencia.FechaFin", "La fecha de fin no puede ser anterior a la de inicio.");
            await OnGetAsync();
            return Page();
        }

        var usuario = User.Identity?.Name ?? "Admin";
        await _ausenciaService.CreateAsync(NuevaAusencia, usuario);

        TempData["Mensaje"] = "Ausencia registrada correctamente.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEliminarAsync(Guid id)
    {
        await _ausenciaService.DeleteAsync(id);
        TempData["Mensaje"] = "Ausencia eliminada.";
        return RedirectToPage();
    }
}