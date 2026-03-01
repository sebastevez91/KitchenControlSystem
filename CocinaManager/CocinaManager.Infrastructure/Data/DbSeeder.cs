using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using CocinaManager.Application.Services;

namespace CocinaManager.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(CocinaDbContext context)
    {
        if (context.Usuarios.Any()) return;

        var usuarios = new List<Usuario>
        {
            new("admin", UsuarioService.HashPassword("admin123"), RolUsuario.Admin),
            new("operario", UsuarioService.HashPassword("operario123"), RolUsuario.Operario)
        };

        await context.Usuarios.AddRangeAsync(usuarios);
        await context.SaveChangesAsync();
    }
}