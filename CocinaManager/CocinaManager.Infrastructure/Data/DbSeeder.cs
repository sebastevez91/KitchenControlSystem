using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using CocinaManager.Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CocinaManager.Infrastructure.Data;

public static class DbSeeder
{
    // Guid fijo para la categoría por defecto (opcional pero recomendable)
    public static readonly Guid SinCategoriaId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public static async Task SeedAsync(CocinaDbContext context)
    {
        if (!context.Usuarios.Any())
        {
            var usuarios = new List<Usuario>
            {
                new Usuario("admin", UsuarioService.HashPassword("admin123"), RolUsuario.Admin, "Administrador", "Sistema"),
                new Usuario("operario", UsuarioService.HashPassword("operario123"), RolUsuario.Operario, "Operario", null)
            };

            await context.Usuarios.AddRangeAsync(usuarios);
            await context.SaveChangesAsync();
        }

        // Asegurar existencia de la categoría "SinCategoria"
        var sinCat = await context.Categorias
            .FirstOrDefaultAsync(c => c.TipoCategoria == TipoCategoria.Sin_Categoria);

        if (sinCat == null)
        {
            // Intentamos usar un Id estable para facilitar futuras migraciones
            sinCat = new Categoria(TipoCategoria.Sin_Categoria, "Categoría por defecto para productos existentes")
            {
                // forzar Id estable (no hace falta más porque asignamos vía reflexión)
            };
            // Si queremos forzar el Id conocido (opcional):
            typeof(Categoria).GetProperty("Id")!.SetValue(sinCat, SinCategoriaId);

            await context.Categorias.AddAsync(sinCat);
            await context.SaveChangesAsync();
        }

        // Asegurar que existan categorías para todos los valores del enum (excepto SinCategoria ya creado)
        var tiposEnum = Enum.GetValues<TipoCategoria>().Cast<TipoCategoria>().Where(t => t != TipoCategoria.Sin_Categoria);
        foreach (var tipo in tiposEnum)
        {
            var existe = await context.Categorias.AnyAsync(c => c.TipoCategoria == tipo);
            if (!existe)
            {
                var nueva = new Categoria(tipo);
                await context.Categorias.AddAsync(nueva);
            }
        }
        await context.SaveChangesAsync();

        // Actualizar productos que no tengan categoría asignada (CategoriaId == Guid.Empty)
        var productosSinCategoria = await context.Productos
            .Where(p => p.CategoriaId == Guid.Empty)
            .ToListAsync();

        if (productosSinCategoria.Any())
        {
            foreach (var p in productosSinCategoria)
                p.AsignarCategoria(sinCat.Id);

            await context.SaveChangesAsync();
        }
    }
}