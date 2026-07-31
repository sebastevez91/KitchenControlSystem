using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CocinaManager.API.Pages.Recepciones;

public class CreateModel : PageModel
{
    private readonly IRecepcionViveresService _recepcionService;
    private readonly IProductoService _productoService;

    [BindProperty] public CreateRecepcionViveresDto NuevaRecepcion { get; set; } = new();
    [BindProperty] public string LineasJson { get; set; } = "[]";

    // Lista de productos para el select/autocomplete
    public List<ProductoDto> Productos { get; set; } = new();

    public CreateModel(IRecepcionViveresService recepcionService, IProductoService productoService)
    {
        _recepcionService = recepcionService;
        _productoService = productoService;
    }

    public async Task OnGetAsync()
    {
        NuevaRecepcion.Fecha = DateTime.Today;
        Productos = await _productoService.GetAllAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Deserialize líneas
        if (!string.IsNullOrEmpty(LineasJson))
        {
            try
            {
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var lineas = JsonSerializer.Deserialize<List<RecepcionViveresLineaDto>>(LineasJson, opciones);
                if (lineas != null) NuevaRecepcion.Lineas = lineas;
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error al procesar las líneas.");
            }
        }

        // Validaciones servidor
        if (NuevaRecepcion.Lineas == null || !NuevaRecepcion.Lineas.Any())
        {
            ModelState.AddModelError(string.Empty, "Debe agregar al menos una línea.");
        }
        else
        {
            for (int i = 0; i < NuevaRecepcion.Lineas.Count; i++)
            {
                var l = NuevaRecepcion.Lineas[i];
                if (l.ProductoId == Guid.Empty)
                    ModelState.AddModelError(string.Empty, $"Línea {i + 1}: producto requerido.");
                if (l.Cantidad <= 0)
                    ModelState.AddModelError(string.Empty, $"Línea {i + 1}: cantidad debe ser mayor a 0.");
            }
        }

        if (!ModelState.IsValid)
        {
            Productos = await _productoService.GetAllAsync();
            return Page();
        }

        var created = await _recepcionService.CreateAsync(NuevaRecepcion);

        // Redirigir a endpoint que devuelve PDF de la recepción
        return Redirect($"/api/recepcion/{created.Id}/pdf");
    }
}