using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IMovimientoStockRepository
{
    Task<List<MovimientoStock>> GetAllAsync();
    Task<List<MovimientoStock>> GetByProductoIdAsync(Guid productoId);
    Task AddAsync(MovimientoStock movimiento);
    Task SaveChangesAsync();
}