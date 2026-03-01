using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IAusenciaRepository
{
    Task<List<Ausencia>> GetAllAsync();
    Task<List<Ausencia>> GetByPersonalIdAsync(Guid personalId);
    Task<Ausencia?> GetByIdAsync(Guid id);
    Task AddAsync(Ausencia ausencia);
    void Remove(Ausencia ausencia);
    Task SaveChangesAsync();
}