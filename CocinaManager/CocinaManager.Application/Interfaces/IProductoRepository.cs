using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IProductoRepository
{
    Task<List<Producto>> GetAllAsync();
    Task<Producto?> GetByIdAsync(Guid id);
    Task<List<Producto>> GetStockBajoAsync();
    Task AddAsync(Producto producto);
    void Remove(Producto producto);
    Task SaveChangesAsync();
}