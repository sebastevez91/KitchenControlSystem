using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IPlanMenuService
{
    Task<List<PlanMenuDto>> GetByRangoAsync(DateTime inicio, DateTime fin);
    Task<List<PlanMenuDto>> GetByFechaAsync(DateTime fecha);
    Task<PlanMenuDto> CreateAsync(CreatePlanMenuDto dto);
    Task<bool> DeleteAsync(Guid id);
}