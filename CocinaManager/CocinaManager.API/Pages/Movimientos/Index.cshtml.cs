using System.Globalization;
using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CocinaManager.Domain.Enums;

namespace CocinaManager.API.Pages.Movimientos;

public class IndexModel : PageModel
{
    private readonly IProductoService _productoService;
    private readonly IMovimientoStockService _movimientoService;
    private readonly CocinaDbContext _context;

    public List<ProductoDto> Productos { get; set; } = new();
    public List<MovimientoStockDto> Movimientos { get; set; } = new();

    // Fecha a filtrar (por defecto hoy)
    [BindProperty(SupportsGet = true)]
    public DateTime Fecha { get; set; } = DateTime.Today;

    // Bind para leer/escribir desde QueryString (GET) y mantener selección en la vista
    [BindProperty(SupportsGet = true)]
    public string FiltroTipo { get; set; } = "Todos";

    [BindProperty] public CreateMovimientoStockDto NuevoMovimiento { get; set; } = new();

    public IndexModel(IMovimientoStockService movimientoService, CocinaDbContext context, IProductoService productoService)
    {
        _movimientoService = movimientoService;
        _context = context;
        _productoService = productoService;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Movimientos";
        Productos = await _productoService.GetAllAsync();

        // Cargar movimientos del día seleccionado, con filtro por tipo si aplica
        if (!string.IsNullOrEmpty(FiltroTipo) && FiltroTipo != "Todos"
            && Enum.TryParse<TipoMovimiento>(FiltroTipo, true, out var tipo))
        {
            Movimientos = await _movimientoService.GetByFechaTipoAsync(Fecha.Date, tipo);
        }
        else
        {
            Movimientos = await _movimientoService.GetByFechaAsync(Fecha.Date);
        }
    }

    public async Task<IActionResult> OnPostRegistrarMovimientoAsync()
    {
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevoMovimiento"))
            .ToList();

        foreach (var key in keysToRemove)
            ModelState.Remove(key);

        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        await _movimientoService.RegistrarMovimientoAsync(NuevoMovimiento);
        TempData["Mensaje"] = $"Movimiento registrado correctamente.";
        return RedirectToPage(new { Fecha = Fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), FiltroTipo });
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var ok = await _movimientoService.DeleteAsync(id);
        TempData["Mensaje"] = ok ? "Movimiento deshecho correctamente." : "No se pudo deshacer el movimiento.";
        return RedirectToPage(new { Fecha = Fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), FiltroTipo });
    }
}
