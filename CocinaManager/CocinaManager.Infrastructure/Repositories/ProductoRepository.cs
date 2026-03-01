using Microsoft.EntityFrameworkCore;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;

namespace CocinaManager.Infrastructure.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly CocinaDbContext _context;

    public ProductoRepository(CocinaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>> GetAllAsync()
        => await _context.Productos.ToListAsync();

    public async Task<Producto?> GetByIdAsync(Guid id)
        => await _context.Productos.FindAsync(id);

    public async Task<List<Producto>> GetStockBajoAsync()
        => await _context.Productos
            .Where(p => p.StockActual <= p.StockMinimo && p.Activo)
            .ToListAsync();

    public async Task AddAsync(Producto producto)
        => await _context.Productos.AddAsync(producto);

    public void Remove(Producto producto)
        => _context.Productos.Remove(producto);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}