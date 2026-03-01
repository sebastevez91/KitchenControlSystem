using CocinaManager.Infrastructure.Data;
using CocinaManager.Application.Services;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.API.Pages;

[Authorize(Roles = "Admin")]
public class UsuariosModel : PageModel
{
    private readonly CocinaDbContext _context;

    public List<Usuario> Usuarios { get; set; } = new();

    [BindProperty] public string NombreUsuario { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty] public RolUsuario Rol { get; set; }

    public UsuariosModel(CocinaDbContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "";
        Usuarios = await _context.Usuarios.ToListAsync();
    }

    public async Task<IActionResult> OnPostCrearAsync()
    {
        if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(Password))
        {
            TempData["Error"] = "Usuario y contraseña son obligatorios.";
            return RedirectToPage();
        }

        if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == NombreUsuario))
        {
            TempData["Error"] = "El nombre de usuario ya existe.";
            return RedirectToPage();
        }

        var usuario = new Usuario(NombreUsuario, UsuarioService.HashPassword(Password), Rol);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Usuario '{NombreUsuario}' creado correctamente.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEliminarAsync(Guid id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Usuario eliminado.";
        }
        return RedirectToPage();
    }
}