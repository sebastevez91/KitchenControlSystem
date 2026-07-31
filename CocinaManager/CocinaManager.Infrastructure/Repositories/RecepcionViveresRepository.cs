using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace CocinaManager.Infrastructure.Repositories;

public class RecepcionViveresRepository : IRecepcionViveresRepository
{
    private readonly CocinaDbContext _context;

    public RecepcionViveresRepository(CocinaDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RecepcionViveres recepcion)
    {
        await _context.RecepcionesViveres.AddAsync(recepcion);
    }

    public async Task<RecepcionViveres?> GetByIdAsync(Guid id)
    {
        return await _context.RecepcionesViveres
            .Include(r => r.Lineas)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<RecepcionViveres>> GetByFechaAsync(DateTime fecha)
    {
        var inicio = fecha.Date;
        var fin = inicio.AddDays(1);
        return await _context.RecepcionesViveres
            .Include(r => r.Lineas)
            .Where(r => r.Fecha >= inicio && r.Fecha < fin)
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var recepcion = await _context.RecepcionesViveres
            .Include(r => r.Lineas)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recepcion != null)
            _context.RecepcionesViveres.Remove(recepcion);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task<byte[]> GeneratePdfFromDtoAsync(RecepcionViveresDto dto)
    {
        // Implementación de ejemplo, debe reemplazarse por la lógica real de generación de PDF
        // Por ahora, solo retorna un array vacío para cumplir con la interfaz
        return Task.FromResult(Array.Empty<byte>());
    }
}