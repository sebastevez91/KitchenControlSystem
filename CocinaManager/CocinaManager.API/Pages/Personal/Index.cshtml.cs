using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CocinaManager.API.Pages.Personal;

public class IndexModel : PageModel
{
    private readonly IPersonalService _personalService;
    private readonly ITurnoService _turnoService;

    public List<PersonalDto> Personal { get; set; } = new();
    public List<TurnoDto> Turnos { get; set; } = new();

    [BindProperty] public CreatePersonalDto NuevoPersonal { get; set; } = new();
    [BindProperty] public CreateTurnoDto NuevoTurno { get; set; } = new();

    public string? Mensaje { get; set; }
    public bool EsError { get; set; }

    public IndexModel(IPersonalService personalService, ITurnoService turnoService)
    {
        _personalService = personalService;
        _turnoService = turnoService;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Personal";
        Personal = await _personalService.GetAllAsync();
        Turnos = await _turnoService.GetAllAsync();
    }

    public async Task<IActionResult> OnPostCrearPersonalAsync()
    {
        // Forzar limpieza de todo lo que no sea NuevoPersonal
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevoPersonal"))
            .ToList();

        foreach (var key in keysToRemove)
            ModelState.Remove(key);

        Console.WriteLine($"ModelState válido después de limpieza: {ModelState.IsValid}");

        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        await _personalService.CreateAsync(NuevoPersonal);
        TempData["Mensaje"] = "Personal creado correctamente.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCrearTurnoAsync()
    {
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevoTurno"))
            .ToList();

        foreach (var key in keysToRemove)
            ModelState.Remove(key);

        Console.WriteLine($"ModelState válido después de limpieza: {ModelState.IsValid}");

        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState)
                foreach (var e in error.Value.Errors)
                    Console.WriteLine($"Campo: {error.Key} - Error: {e.ErrorMessage}");

            await OnGetAsync();
            return Page();
        }

        await _turnoService.CreateAsync(NuevoTurno);
        TempData["Mensaje"] = "Turno asignado correctamente.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCambiarEstadoAsync(Guid id, EstadoPersonal estado)
    {
        await _personalService.CambiarEstadoAsync(id, estado);
        TempData["Mensaje"] = "Estado actualizado.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEliminarPersonalAsync(Guid id)
    {
        await _personalService.DeleteAsync(id);
        TempData["Mensaje"] = "Personal eliminado.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEliminarTurnoAsync(Guid id)
    {
        await _turnoService.DeleteAsync(id);
        TempData["Mensaje"] = "Turno eliminado.";
        return RedirectToPage();
    }
}