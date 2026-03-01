using Microsoft.EntityFrameworkCore;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;

namespace CocinaManager.Infrastructure.Repositories;

public class TurnoRepository : ITurnoRepository
{
    private readonly CocinaDbContext _context;

    public TurnoRepository(CocinaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Turno>> GetAllAsync()
        => await _context.Turnos.Include(t => t.Personal).ToListAsync();

    public async Task<List<Turno>> GetByPersonalIdAsync(Guid personalId)
        => await _context.Turnos
            .Include(t => t.Personal)
            .Where(t => t.PersonalId == personalId)
            .ToListAsync();

    public async Task<Turno?> GetByIdAsync(Guid id)
        => await _context.Turnos.Include(t => t.Personal).FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(Turno turno)
        => await _context.Turnos.AddAsync(turno);

    public void Remove(Turno turno)
        => _context.Turnos.Remove(turno);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<List<Turno>> GetByFechaAsync(DateTime fecha)
    => await _context.Turnos
        .Include(t => t.Personal)
        .Where(t => t.Fecha.Date == fecha.Date)
        .ToListAsync();

    public async Task<List<Turno>> GetByRangoAsync(DateTime inicio, DateTime fin)
    => await _context.Turnos
        .Include(t => t.Personal)
        .Where(t => t.Fecha.Date >= inicio.Date && t.Fecha.Date <= fin.Date)
        .ToListAsync();
}