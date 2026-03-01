using Microsoft.EntityFrameworkCore;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using CocinaManager.Infrastructure.Data;

namespace CocinaManager.Infrastructure.Repositories;

public class HerramientaRepository : IHerramientaRepository
{
    private readonly CocinaDbContext _context;

    public HerramientaRepository(CocinaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Herramienta>> GetAllAsync()
        => await _context.Herramientas.Where(h => h.Activo).ToListAsync();

    public async Task<Herramienta?> GetByIdAsync(Guid id)
        => await _context.Herramientas.FindAsync(id);

    public async Task<List<Herramienta>> GetByEstadoAsync(EstadoHerramienta estado)
        => await _context.Herramientas.Where(h => h.Estado == estado && h.Activo).ToListAsync();

    public async Task AddAsync(Herramienta herramienta)
        => await _context.Herramientas.AddAsync(herramienta);

    public void Remove(Herramienta herramienta)
        => herramienta.Desactivar(); // Soft delete

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}