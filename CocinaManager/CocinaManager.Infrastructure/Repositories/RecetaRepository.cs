using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.Infrastructure.Repositories;

public class RecetaRepository : IRecetaRepository
{
    private readonly CocinaDbContext _context;

    public RecetaRepository(CocinaDbContext context) => _context = context;

    public async Task<List<Receta>> GetAllAsync(bool incluirInactivas = false)
        => await _context.Recetas
            .Include(r => r.Ingredientes)
            .Where(r => incluirInactivas || r.Activo)
            .OrderBy(r => r.Nombre)
            .ToListAsync();

    public async Task<Receta?> GetByIdAsync(Guid id)
        => await _context.Recetas
            .Include(r => r.Ingredientes)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task AddAsync(Receta receta)
        => await _context.Recetas.AddAsync(receta);

    public void Remove(Receta receta) => receta.Desactivar();

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<List<RecetaSimpleDto>> GetAllSimpleAsync() =>
    await _context.Recetas
             .OrderBy(r => r.Nombre)
             .Select(r => new RecetaSimpleDto { Id = r.Id, Nombre = r.Nombre })
             .ToListAsync();

    public async Task ReemplazarIngredientesAsync(Guid recetaId, List<RecetaIngrediente> nuevos)
    {
        var actuales = await _context.RecetaIngredientes
            .Where(i => i.RecetaId == recetaId)
            .ToListAsync();

        _context.RecetaIngredientes.RemoveRange(actuales);
        await _context.RecetaIngredientes.AddRangeAsync(nuevos);
        await _context.SaveChangesAsync();
    }
}