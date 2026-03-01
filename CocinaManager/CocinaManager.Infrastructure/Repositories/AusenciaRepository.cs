using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.Infrastructure.Repositories;

public class AusenciaRepository : IAusenciaRepository
{
    private readonly CocinaDbContext _context;

    public AusenciaRepository(CocinaDbContext context) => _context = context;

    public async Task<List<Ausencia>> GetAllAsync()
        => await _context.Ausencias
            .Include(a => a.Personal)
            .OrderByDescending(a => a.FechaInicio)
            .ToListAsync();

    public async Task<List<Ausencia>> GetByPersonalIdAsync(Guid personalId)
        => await _context.Ausencias
            .Include(a => a.Personal)
            .Where(a => a.PersonalId == personalId)
            .OrderByDescending(a => a.FechaInicio)
            .ToListAsync();

    public async Task<Ausencia?> GetByIdAsync(Guid id)
        => await _context.Ausencias.FindAsync(id);

    public async Task AddAsync(Ausencia ausencia)
        => await _context.Ausencias.AddAsync(ausencia);

    public void Remove(Ausencia ausencia)
        => _context.Ausencias.Remove(ausencia);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}