using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Interfaces;

public interface IPlanMenuRepository
{
    Task<List<PlanMenu>> GetAllWithItemsAsync();
    Task<PlanMenu?> GetByIdWithItemsAsync(Guid id);
    Task<PlanMenu?> GetByDiaYTipoAsync(DiaSemana diaSemana, TipoMenu tipoMenu);
    Task AddAsync(PlanMenu plan);
    Task RemoveAsync(PlanMenu plan);
    Task SaveChangesAsync();
    Task ReemplazarItemAsync(Guid planMenuId, Guid recetaId, RolPlato? rol);
    Task LimpiarItemsAsync(Guid planMenuId, RolPlato? rol);
}