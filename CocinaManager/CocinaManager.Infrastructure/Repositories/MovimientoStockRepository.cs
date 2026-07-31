using Microsoft.EntityFrameworkCore;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
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

    public async Task<List<MovimientoStock>> GetByFechaAsync(DateTime fecha)
    {
        var inicio = fecha.Date;
        var fin = inicio.AddDays(1);
        return await _context.MovimientosStock
            .Include(m => m.Producto)
            .Where(m => m.Fecha >= inicio && m.Fecha < fin)
            .ToListAsync();
    }

    public async Task<List<MovimientoStock>> GetByFechaTipoAsync(DateTime fecha, TipoMovimiento tipo)
    {
        var inicio = fecha.Date;
        var fin = inicio.AddDays(1);
        return await _context.MovimientosStock
            .Include(m => m.Producto)
            .Where(m => m.Fecha >= inicio && m.Fecha < fin && m.TipoMovimiento == tipo)
            .ToListAsync();
    }

    public async Task AddAsync(MovimientoStock movimiento)
        => await _context.MovimientosStock.AddAsync(movimiento);

    public async Task RemoveAsync(MovimientoStock movimiento)
    {
        _context.MovimientosStock.Remove(movimiento);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}