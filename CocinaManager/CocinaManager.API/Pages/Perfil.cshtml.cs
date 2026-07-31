using CocinaManager.Infrastructure.Data;
using CocinaManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.API.Pages;

[Authorize]
public class PerfilModel : PageModel
{
    private readonly CocinaDbContext _context;

    [BindProperty] public string Nombre { get; set; } = string.Empty;
    [BindProperty] public string Apellido { get; set; } = string.Empty;

    public PerfilModel(CocinaDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return Challenge();

        var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.NombreUsuario == username);
        if (usuario == null)
            return NotFound();

        Nombre = usuario.Nombre ?? string.Empty;
        Apellido = usuario.Apellido ?? string.Empty;
        return Page();
    }

    public async Task<IActionResult> OnPostGuardarNombreAsync()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return Challenge();

        var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.NombreUsuario == username);
        if (usuario == null)
            return NotFound();

        usuario.ActualizarNombreApellido(string.IsNullOrWhiteSpace(Nombre) ? null : Nombre,
                                         string.IsNullOrWhiteSpace(usuario.Apellido) ? null : usuario.Apellido);

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Nombre actualizado correctamente.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostGuardarApellidoAsync()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return Challenge();

        var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.NombreUsuario == username);
        if (usuario == null)
            return NotFound();

        usuario.ActualizarNombreApellido(string.IsNullOrWhiteSpace(usuario.Nombre) ? null : usuario.Nombre,
                                         string.IsNullOrWhiteSpace(Apellido) ? null : Apellido);

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Apellido actualizado correctamente.";
        return RedirectToPage();
    }
}