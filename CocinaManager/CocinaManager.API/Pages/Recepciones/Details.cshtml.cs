using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CocinaManager.Application.Interfaces;
using CocinaManager.Application.DTOs;

namespace CocinaManager.API.Pages.Recepciones;

public class DetailsModel : PageModel
{
    private readonly IRecepcionViveresService _recepcionService;

    public DetailsModel(IRecepcionViveresService recepcionService)
    {
        _recepcionService = recepcionService;
    }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public RecepcionViveresDto? Recepcion { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Recepcion = await _recepcionService.GetByIdAsync(id);
        if (Recepcion == null) return NotFound();
        return Page();
    }
}
