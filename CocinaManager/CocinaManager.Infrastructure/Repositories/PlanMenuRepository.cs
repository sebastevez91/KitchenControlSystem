using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.Infrastructure.Repositories;

public class PlanMenuRepository : IPlanMenuRepository
{
    private readonly CocinaDbContext _context;

    public PlanMenuRepository(CocinaDbContext context) => _context = context;

    public async Task<List<PlanMenu>> GetByRangoAsync(DateTime inicio, DateTime fin)
        => await _context.PlanesMenu
            .Include(p => p.Receta)
            .Where(p => p.Fecha.Date >= inicio.Date && p.Fecha.Date <= fin.Date)
            .OrderBy(p => p.Fecha)
            .ToListAsync();

    public async Task<List<PlanMenu>> GetByFechaAsync(DateTime fecha)
        => await _context.PlanesMenu
            .Include(p => p.Receta)
            .Where(p => p.Fecha.Date == fecha.Date)
            .ToListAsync();

    public async Task<PlanMenu?> GetByIdAsync(Guid id)
        => await _context.PlanesMenu.FindAsync(id);

    public async Task AddAsync(PlanMenu plan)
        => await _context.PlanesMenu.AddAsync(plan);

    public void Remove(PlanMenu plan) => _context.PlanesMenu.Remove(plan);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}