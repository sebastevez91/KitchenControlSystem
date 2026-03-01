using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CocinaManager.API.Pages.Recetas;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IRecetaService _recetaService;
    private readonly IPlanMenuService _planService;

    public List<RecetaDto> Recetas { get; set; } = new();
    public List<PlanMenuDto> MenuSemana { get; set; } = new();
    public DateTime InicioSemana { get; set; }
    public DateTime FinSemana { get; set; }

    [BindProperty] public CreateRecetaDto NuevaReceta { get; set; } = new();
    [BindProperty] public CreatePlanMenuDto NuevoPlan { get; set; } = new();

    // Ingredientes temporales via JSON
    [BindProperty] public string IngredientesJson { get; set; } = "[]";

    public IndexModel(IRecetaService recetaService, IPlanMenuService planService)
    {
        _recetaService = recetaService;
        _planService = planService;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Recetas";
        Recetas = await _recetaService.GetAllAsync();

        // Semana actual
        var hoy = DateTime.Today;
        InicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek + (int)DayOfWeek.Monday);
        FinSemana = InicioSemana.AddDays(6);
        MenuSemana = await _planService.GetByRangoAsync(InicioSemana, FinSemana);
    }

    public async Task<IActionResult> OnPostCrearRecetaAsync()
    {
        try
        {
            var keysToRemove = ModelState.Keys
                .Where(k => !k.StartsWith("NuevaReceta") && k != "IngredientesJson")
                .ToList();
            foreach (var key in keysToRemove) ModelState.Remove(key);

            if (!string.IsNullOrEmpty(IngredientesJson))
            {
                var opciones = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var ingredientes = System.Text.Json.JsonSerializer
                    .Deserialize<List<CreateRecetaIngredienteDto>>(IngredientesJson, opciones);

                if (ingredientes != null)
                    NuevaReceta.Ingredientes = ingredientes;
            }

            Console.WriteLine($"ModelState válido: {ModelState.IsValid}");
            Console.WriteLine($"Nombre: {NuevaReceta.Nombre}");
            Console.WriteLine($"Porciones: {NuevaReceta.Porciones}");
            Console.WriteLine($"IngredientesJson: {IngredientesJson}");

            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        Console.WriteLine($"Error campo {e.Key}: {err.ErrorMessage}");

                await OnGetAsync();
                return Page();
            }

            await _recetaService.CreateAsync(NuevaReceta);
            TempData["Mensaje"] = "Receta creada correctamente.";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"EXCEPCION: {ex.Message}");
            Console.WriteLine($"INNER: {ex.InnerException?.Message}");
            Console.WriteLine($"STACK: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<IActionResult> OnPostPlanificarAsync()
    {
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevoPlan"))
            .ToList();
        foreach (var key in keysToRemove) ModelState.Remove(key);

        if (!ModelState.IsValid) { await OnGetAsync(); return Page(); }

        await _planService.CreateAsync(NuevoPlan);
        TempData["Mensaje"] = "Menú planificado correctamente.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEliminarRecetaAsync(Guid id)
    {
        await _recetaService.DeleteAsync(id);
        TempData["Mensaje"] = "Receta eliminada.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEliminarPlanAsync(Guid id)
    {
        await _planService.DeleteAsync(id);
        TempData["Mensaje"] = "Menú eliminado del plan.";
        return RedirectToPage();
    }
}