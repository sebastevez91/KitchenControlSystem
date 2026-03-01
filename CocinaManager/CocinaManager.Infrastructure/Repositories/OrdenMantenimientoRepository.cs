using Microsoft.EntityFrameworkCore;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;

namespace CocinaManager.Infrastructure.Repositories;

public class OrdenMantenimientoRepository : IOrdenMantenimientoRepository
{
    private readonly CocinaDbContext _context;

    public OrdenMantenimientoRepository(CocinaDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrdenMantenimiento>> GetAllAsync()
        => await _context.OrdenesMantenimiento.Include(o => o.Herramienta).ToListAsync();

    public async Task<List<OrdenMantenimiento>> GetByHerramientaIdAsync(Guid herramientaId)
        => await _context.OrdenesMantenimiento
            .Include(o => o.Herramienta)
            .Where(o => o.HerramientaId == herramientaId)
            .ToListAsync();

    public async Task<OrdenMantenimiento?> GetByIdAsync(Guid id)
        => await _context.OrdenesMantenimiento
            .Include(o => o.Herramienta)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task AddAsync(OrdenMantenimiento orden)
        => await _context.OrdenesMantenimiento.AddAsync(orden);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<List<OrdenMantenimiento>> GetPendientesAsync()
    => await _context.OrdenesMantenimiento
        .Include(o => o.Herramienta)
        .Where(o => o.EstadoOrden == Domain.Enums.EstadoOrden.Pendiente
                 || o.EstadoOrden == Domain.Enums.EstadoOrden.EnProceso)
        .OrderBy(o => o.FechaCreacion)
        .ToListAsync();
}