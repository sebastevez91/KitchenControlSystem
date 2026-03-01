using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using CocinaManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.Infrastructure.Repositories;

public class IncidenteRepository : IIncidenteRepository
{
    private readonly CocinaDbContext _context;

    public IncidenteRepository(CocinaDbContext context) => _context = context;

    public async Task<List<Incidente>> GetAllAsync()
        => await _context.Incidentes
            .OrderByDescending(i => i.FechaRegistro)
            .ToListAsync();

    public async Task<List<Incidente>> GetByEstadoAsync(EstadoIncidente estado)
        => await _context.Incidentes
            .Where(i => i.Estado == estado)
            .OrderByDescending(i => i.FechaRegistro)
            .ToListAsync();

    public async Task<Incidente?> GetByIdAsync(Guid id)
        => await _context.Incidentes.FindAsync(id);

    public async Task AddAsync(Incidente incidente)
        => await _context.Incidentes.AddAsync(incidente);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}