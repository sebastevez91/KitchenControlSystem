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

    public IndexModel(IRecetaService recetaService) => _recetaService = recetaService;

    public List<RecetaDto> Recetas { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Busqueda { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool MostrarInactivas { get; set; } = false;

    public bool EsAdmin => User.IsInRole("Admin");

    public async Task OnGetAsync()
    {
        var todas = await _recetaService.GetAllAsync(MostrarInactivas);
        Recetas = string.IsNullOrWhiteSpace(Busqueda)
            ? todas
            : todas.Where(r => r.Nombre.Contains(Busqueda, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    // ---- Handlers AJAX (usados por el modal) ----

    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> OnGetObtenerAsync(Guid id)
    {
        var receta = await _recetaService.GetByIdAsync(id);
        return receta == null ? NotFound() : new JsonResult(receta);
    }

    public async Task<IActionResult> OnPostCrearAsync([FromBody] CreateRecetaDto dto)
    {
        if (!User.IsInRole("Admin")) return Forbid();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var creada = await _recetaService.CreateAsync(dto);
        return new JsonResult(creada);
    }

    public async Task<IActionResult> OnPostActualizarAsync([FromBody] UpdateRecetaDto dto)
    {
        if (!User.IsInRole("Admin")) return Forbid();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var actualizada = await _recetaService.UpdateAsync(dto);
        return new JsonResult(actualizada);
    }

    public async Task<IActionResult> OnPostToggleAsync([FromForm] Guid id)
    {
        if (!User.IsInRole("Admin")) return Forbid();
        var ok = await _recetaService.ToggleAcitvoAsync(id);
        return ok ? new JsonResult(new { success = true }) : NotFound();
    }
}