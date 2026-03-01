using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CocinaManager.Domain.Enums;

namespace CocinaManager.API.Pages.Deposito;

public class IndexModel : PageModel
{
    private readonly IProductoService _productoService;
    private readonly IMovimientoStockService _movimientoService;

    public List<ProductoDto> Productos { get; set; } = new();
    public List<MovimientoStockDto> Movimientos { get; set; } = new();
    public List<ProductoDto> ProductosStockBajo { get; set; } = new();

    [BindProperty] public CreateProductoDto NuevoProducto { get; set; } = new();
    [BindProperty] public CreateMovimientoStockDto NuevoMovimiento { get; set; } = new();

    public IndexModel(IProductoService productoService, IMovimientoStockService movimientoService)
    {
        _productoService = productoService;
        _movimientoService = movimientoService;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Deposito";
        Productos = await _productoService.GetAllAsync();
        Movimientos = await _movimientoService.GetAllAsync();
        ProductosStockBajo = await _productoService.GetStockBajoAsync();
    }

    public async Task<IActionResult> OnPostCrearProductoAsync()
    {
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevoProducto"))
            .ToList();

        foreach (var key in keysToRemove)
            ModelState.Remove(key);

        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        await _productoService.CreateAsync(NuevoProducto);
        TempData["Mensaje"] = "Producto creado correctamente.";
        return RedirectToPage();
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
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEliminarProductoAsync(Guid id)
    {
        await _productoService.DeleteAsync(id);
        TempData["Mensaje"] = "Producto eliminado.";
        return RedirectToPage();
    }
}