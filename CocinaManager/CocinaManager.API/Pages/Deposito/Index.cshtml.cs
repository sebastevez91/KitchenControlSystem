using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CocinaManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CocinaManager.Infrastructure.Data;

namespace CocinaManager.API.Pages.Deposito;

public class IndexModel : PageModel
{
    private readonly IProductoService _productoService;
    private readonly CocinaDbContext _context;

    public List<ProductoDto> Productos { get; set; } = new();
    public List<ProductoDto> ProductosStockBajo { get; set; } = new();

    [BindProperty] public CreateProductoDto NuevoProducto { get; set; } = new();
    [BindProperty] public UpdateProductoDto EditProducto { get; set; } = new();

    public List<Categoria> Categorias { get; set; } = new List<Categoria>();

    public IndexModel(IProductoService productoService, IMovimientoStockService movimientoService, CocinaDbContext context)
    {
        _productoService = productoService;
        _context = context;
    }

    // Añadimos parámetro opcional 'q' para búsqueda por nombre
    public async Task OnGetAsync(string? q)
    {
        ViewData["ActivePage"] = "Deposito";

        var all = await _productoService.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var qnorm = q.Trim().ToLowerInvariant();
            Productos = all.Where(p => p.Nombre != null && p.Nombre.ToLowerInvariant().Contains(qnorm)).ToList();
            ViewData["SearchQuery"] = q;
        }
        else
        {
            Productos = all;
        }

        ProductosStockBajo = await _productoService.GetStockBajoAsync();

        // Poblar categorías desde la base de datos para selects
        Categorias = await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync();
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
            await OnGetAsync(null);
            return Page();
        }

        await _productoService.CreateAsync(NuevoProducto);
        TempData["Mensaje"] = "Producto creado correctamente.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEliminarProductoAsync(Guid id)
    {
        await _productoService.DeleteAsync(id);
        TempData["Mensaje"] = "Producto eliminado.";
        return RedirectToPage();
    }

    // Nuevo handler para editar producto desde el modal.
    public async Task<IActionResult> OnPostEditarProductoAsync()
    {
        // Validar solo campos del EditProducto
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("EditProducto"))
            .ToList();

        foreach (var key in keysToRemove)
            ModelState.Remove(key);

        if (!ModelState.IsValid)
        {
            await OnGetAsync(null);
            return Page();
        }

        var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == EditProducto.Id);
        if (producto == null)
        {
            TempData["Mensaje"] = "Producto no encontrado.";
            return RedirectToPage();
        }

        producto.Editar(EditProducto.Nombre, EditProducto.UnidadMedida, EditProducto.StockMinimo, EditProducto.CategoriaId);

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Producto actualizado correctamente.";
        return RedirectToPage();
    }
}