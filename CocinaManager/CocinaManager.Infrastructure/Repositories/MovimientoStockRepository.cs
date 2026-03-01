using Microsoft.EntityFrameworkCore;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;

namespace CocinaManager.Infrastructure.Repositories;

public class MovimientoStockRepository : IMovimientoStockRepository
{
    private readonly CocinaDbContext _context;

    public MovimientoStockRepository(CocinaDbContext context)
    {
        _context = context;
    }

    public async Task<List<MovimientoStock>> GetAllAsync()
        => await _context.MovimientosStock.Include(m => m.Producto).ToListAsync();

    public async Task<List<MovimientoStock>> GetByProductoIdAsync(Guid productoId)
        => await _context.MovimientosStock
            .Include(m => m.Producto)
            .Where(m => m.ProductoId == productoId)
            .ToListAsync();

    public async Task AddAsync(MovimientoStock movimiento)
        => await _context.MovimientosStock.AddAsync(movimiento);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}