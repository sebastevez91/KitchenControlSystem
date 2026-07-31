using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace CocinaManager.API.Pages.Recepciones;

public class IndexModel : PageModel
{
    private readonly IRecepcionViveresService _recepcionService;

    public IndexModel(IRecepcionViveresService recepcionService)
    {
        _recepcionService = recepcionService;
    }

    [BindProperty(SupportsGet = true)]
    public DateTime Fecha { get; set; } = DateTime.Today;

    public List<RecepcionViveresDto> Recepciones { get; set; } = new();

    [TempData]
    public string? Mensaje { get; set; }

    public async Task OnGetAsync(DateTime? fecha)
    {
        Fecha = fecha ?? DateTime.Today;
        Recepciones = await _recepcionService.GetByFechaAsync(Fecha);
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var ok = await _recepcionService.DeleteAsync(id);
        Mensaje = ok ? "Recepción eliminada y stock actualizado." : "No se encontró la recepción.";
        return RedirectToPage(new { fecha = Fecha.ToString("yyyy-MM-dd") });
    }

    public async Task<IActionResult> OnGetDownloadAsync(Guid id)
    {
        var dto = await _recepcionService.GetByIdAsync(id);
        if (dto == null) return NotFound();


        return Redirect($"/api/recepcion/recepcion/{id}/pdf"); ;
    }
}
