using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CocinaManager.API.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IPersonalService _personalService;
    private readonly IProductoService _productoService;
    private readonly IHerramientaService _herramientaService;
    private readonly ITurnoService _turnoService;
    private readonly IOrdenMantenimientoService _ordenService;
    private readonly IIncidenteService _incidenteService;

    // Stats generales
    public int TotalPersonal { get; set; }
    public int ProductosStockBajo { get; set; }
    public int HerramientasEnReparacion { get; set; }
    public int IncidentesAbiertos { get; set; }

    // Dashboard enriquecido
    public string NombreUsuario { get; set; } = string.Empty;
    public string RolUsuario { get; set; } = string.Empty;
    public DateTime FechaHoy { get; set; }
    public List<TurnoDto> TurnosHoy { get; set; } = new();
    public List<OrdenMantenimientoDto> OrdenesPendientes { get; set; } = new();
    public List<PersonalDto> PersonalInactivo { get; set; } = new();

    public IndexModel(
        IPersonalService personalService,
        IProductoService productoService,
        IHerramientaService herramientaService,
        ITurnoService turnoService,
        IOrdenMantenimientoService ordenService,
        IIncidenteService incidenteService)
    {
        _personalService = personalService;
        _productoService = productoService;
        _herramientaService = herramientaService;
        _turnoService = turnoService;
        _ordenService = ordenService;
        _incidenteService = incidenteService;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Dashboard";

        // Info del usuario logueado
        NombreUsuario = User.Identity?.Name ?? "Usuario";
        RolUsuario = User.IsInRole("Admin") ? "Admin" : "Operario";
        FechaHoy = DateTime.Today;

        // Stats
        var personal = await _personalService.GetAllAsync();
        TotalPersonal = personal.Count;
        ProductosStockBajo = (await _productoService.GetStockBajoAsync()).Count;
        HerramientasEnReparacion = (await _herramientaService
            .GetByEstadoAsync(Domain.Enums.EstadoHerramienta.EnReparacion)).Count;

        // Turnos de hoy
        TurnosHoy = await _turnoService.GetByFechaAsync(FechaHoy);

        // Órdenes pendientes
        OrdenesPendientes = await _ordenService.GetPendientesAsync();

        // Personal inactivo (enfermo, ausente, licencia)
        PersonalInactivo = personal
            .Where(p => p.Estado != Domain.Enums.EstadoPersonal.Activo)
            .ToList();

        IncidentesAbiertos = (await _incidenteService.GetByEstadoAsync(Domain.Enums.EstadoIncidente.Abierto)).Count;
    }


}