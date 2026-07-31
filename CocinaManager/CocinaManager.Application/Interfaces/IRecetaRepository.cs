using CocinaManager.Application.DTOs;
using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IRecetaRepository
{
    Task<List<Receta>> GetAllAsync(bool incluirInactivas = false);
    Task<Receta?> GetByIdAsync(Guid id);
    Task AddAsync(Receta receta);
    void Remove(Receta receta);
    Task SaveChangesAsync();
    Task<List<RecetaSimpleDto>> GetAllSimpleAsync();
    Task ReemplazarIngredientesAsync(Guid recetaId, List<RecetaIngrediente> nuevos);
}