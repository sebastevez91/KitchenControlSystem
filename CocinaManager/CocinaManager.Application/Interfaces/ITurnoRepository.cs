using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface ITurnoRepository
{
    Task<List<Turno>> GetAllAsync();
    Task<List<Turno>> GetByPersonalIdAsync(Guid personalId);
    Task<Turno?> GetByIdAsync(Guid id);
    Task AddAsync(Turno turno);
    void Remove(Turno turno);
    Task SaveChangesAsync();
    Task<List<Turno>> GetByFechaAsync(DateTime fecha);
    Task<List<Turno>> GetByRangoAsync(DateTime inicio, DateTime fin);
}