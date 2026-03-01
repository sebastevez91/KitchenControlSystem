using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IPlanMenuRepository
{
    Task<List<PlanMenu>> GetByRangoAsync(DateTime inicio, DateTime fin);
    Task<List<PlanMenu>> GetByFechaAsync(DateTime fecha);
    Task AddAsync(PlanMenu plan);
    Task<PlanMenu?> GetByIdAsync(Guid id);
    void Remove(PlanMenu plan);
    Task SaveChangesAsync();
}