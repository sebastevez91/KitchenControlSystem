using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IOrdenMantenimientoRepository
{
    Task<List<OrdenMantenimiento>> GetAllAsync();
    Task<List<OrdenMantenimiento>> GetByHerramientaIdAsync(Guid herramientaId);
    Task<OrdenMantenimiento?> GetByIdAsync(Guid id);
    Task AddAsync(OrdenMantenimiento orden);
    Task SaveChangesAsync();
    Task<List<OrdenMantenimiento>> GetPendientesAsync();
}