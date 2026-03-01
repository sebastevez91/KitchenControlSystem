using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Interfaces;

public interface IIncidenteRepository
{
    Task<List<Incidente>> GetAllAsync();
    Task<List<Incidente>> GetByEstadoAsync(EstadoIncidente estado);
    Task<Incidente?> GetByIdAsync(Guid id);
    Task AddAsync(Incidente incidente);
    Task SaveChangesAsync();
}