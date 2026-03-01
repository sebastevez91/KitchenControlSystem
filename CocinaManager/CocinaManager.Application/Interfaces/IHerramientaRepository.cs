using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IHerramientaRepository
{
    Task<List<Herramienta>> GetAllAsync();
    Task<Herramienta?> GetByIdAsync(Guid id);
    Task<List<Herramienta>> GetByEstadoAsync(Domain.Enums.EstadoHerramienta estado);
    Task AddAsync(Herramienta herramienta);
    void Remove(Herramienta herramienta);
    Task SaveChangesAsync();
}