using CocinaManager.Application.DTOs;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Interfaces;

public interface IPlanMenuService
{
    Task<List<PlanMenuDto>> GetAllAsync();
    Task<PlanMenuDto?> GetByIdAsync(Guid id);
    Task<PlanMenuDto> CreateAsync(CreatePlanMenuDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> AddRecetaAsync(Guid planId, Guid recetaId);
    Task<bool> RemoveItemAsync(Guid planId, Guid itemId);
    Task SetRecetaAsync(DiaSemana diaSemana, TipoMenu tipoMenu, Guid recetaId, RolPlato? rol = null);
    Task LimpiarCeldaAsync(DiaSemana diaSemana, TipoMenu tipoMenu, RolPlato? rol = null);
}