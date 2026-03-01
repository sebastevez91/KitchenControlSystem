using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.Infrastructure.Repositories;

public class RecetaRepository : IRecetaRepository
{
    private readonly CocinaDbContext _context;

    public RecetaRepository(CocinaDbContext context) => _context = context;

    public async Task<List<Receta>> GetAllAsync()
        => await _context.Recetas
            .Include(r => r.Ingredientes)
            .Where(r => r.Activo)
            .ToListAsync();

    public async Task<Receta?> GetByIdAsync(Guid id)
        => await _context.Recetas
            .Include(r => r.Ingredientes)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task AddAsync(Receta receta)
        => await _context.Recetas.AddAsync(receta);

    public void Remove(Receta receta) => receta.Desactivar();

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}