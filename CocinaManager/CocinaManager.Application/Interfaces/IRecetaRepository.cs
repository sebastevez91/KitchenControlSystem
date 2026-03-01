using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IRecetaRepository
{
    Task<List<Receta>> GetAllAsync();
    Task<Receta?> GetByIdAsync(Guid id);
    Task AddAsync(Receta receta);
    void Remove(Receta receta);
    Task SaveChangesAsync();
}