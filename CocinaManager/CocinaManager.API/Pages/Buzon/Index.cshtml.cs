using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.API.Pages.Buzon;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IMensajeService _mensajeService;
    private readonly CocinaDbContext _context;

    public List<MensajeDto> Recibidos { get; set; } = new();
    public List<MensajeDto> Enviados { get; set; } = new();
    public List<string> Usuarios { get; set; } = new();
    public int NoLeidos { get; set; }
    public string Vista { get; set; } = "recibidos";

    // Para ver detalle
    public MensajeDto? MensajeDetalle { get; set; }

    [BindProperty] public CreateMensajeDto NuevoMensaje { get; set; } = new();
    [BindProperty] public string? CuerpoRespuesta { get; set; }

    public IndexModel(IMensajeService mensajeService, CocinaDbContext context)
    {
        _mensajeService = mensajeService;
        _context = context;
    }

    public async Task OnGetAsync(string? vista, Guid? id)
    {
        ViewData["ActivePage"] = "Buzon";
        var usuario = User.Identity?.Name ?? string.Empty;
        Vista = vista ?? "recibidos";

        Recibidos = await _mensajeService.GetRecibidosAsync(usuario);
        Enviados = await _mensajeService.GetEnviadosAsync(usuario);
        NoLeidos = await _mensajeService.GetNoLeidosCountAsync(usuario);
        Usuarios = await _context.Usuarios
            .Where(u => u.NombreUsuario != usuario && u.Activo)
            .Select(u => u.NombreUsuario)
            .ToListAsync();

        if (id.HasValue)
            MensajeDetalle = await _mensajeService.GetByIdAsync(id.Value, usuario);
    }

    public async Task<IActionResult> OnPostEnviarAsync()
    {
        var keysToRemove = ModelState.Keys
            .Where(k => !k.StartsWith("NuevoMensaje"))
            .ToList();
        foreach (var key in keysToRemove) ModelState.Remove(key);

        if (!ModelState.IsValid)
        {
            await OnGetAsync(null, null);
            return Page();
        }

        var usuario = User.Identity?.Name ?? string.Empty;
        await _mensajeService.EnviarAsync(NuevoMensaje, usuario);
        TempData["Mensaje"] = "Mensaje enviado correctamente.";
        return RedirectToPage(new { vista = "enviados" });
    }

    public async Task<IActionResult> OnPostResponderAsync(Guid mensajePadreId, string cuerpoRespuesta)
    {
        var usuario = User.Identity?.Name ?? string.Empty;
        await _mensajeService.ResponderAsync(mensajePadreId, cuerpoRespuesta, usuario);
        TempData["Mensaje"] = "Respuesta enviada.";
        return RedirectToPage(new { vista = "recibidos" });
    }

    public async Task<IActionResult> OnPostEliminarAsync(Guid id, string vista)
    {
        var usuario = User.Identity?.Name ?? string.Empty;
        await _mensajeService.EliminarAsync(id, usuario);
        TempData["Mensaje"] = "Mensaje eliminado.";
        return RedirectToPage(new { vista });
    }
}