using CocinaManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CocinaManager.API.Pages;

[Authorize]
public class PerfilModel : PageModel
{
    private readonly ITurnoService _turnoService;

    public string NombreUsuario { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public int TurnosHoy { get; set; }
    public int TotalTurnos { get; set; }

    public PerfilModel(ITurnoService turnoService)
    {
        _turnoService = turnoService;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "";
        NombreUsuario = User.Identity?.Name ?? "Usuario";
        Rol = User.IsInRole("Admin") ? "Administrador" : "Operario";
        TurnosHoy = (await _turnoService.GetByFechaAsync(DateTime.Today)).Count;
        TotalTurnos = (await _turnoService.GetAllAsync()).Count;
    }
}