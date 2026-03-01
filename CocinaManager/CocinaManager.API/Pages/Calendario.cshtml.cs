using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CocinaManager.API.Pages;

[Authorize]
public class CalendarioModel : PageModel
{
    private readonly IPersonalService _personalService;
    private readonly ITurnoService _turnoService;

    public List<PersonalDto> Personal { get; set; } = new();
    [BindProperty] public CreateTurnoDto NuevoTurno { get; set; } = new();

    public CalendarioModel(IPersonalService personalService, ITurnoService turnoService)
    {
        _personalService = personalService;
        _turnoService = turnoService;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Calendario";
        Personal = await _personalService.GetAllAsync();
    }

    public async Task<IActionResult> OnPostCrearTurnoAsync()
    {
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevoTurno"))
            .ToList();
        foreach (var key in keysToRemove)
            ModelState.Remove(key);

        if (!ModelState.IsValid)
            return RedirectToPage();

        await _turnoService.CreateAsync(NuevoTurno);
        TempData["Mensaje"] = "Turno asignado correctamente.";
        return RedirectToPage();
    }
}