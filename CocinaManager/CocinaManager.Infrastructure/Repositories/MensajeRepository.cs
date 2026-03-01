using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.Infrastructure.Repositories;

public class MensajeRepository : IMensajeRepository
{
    private readonly CocinaDbContext _context;

    public MensajeRepository(CocinaDbContext context) => _context = context;

    public async Task<List<Mensaje>> GetRecibidosAsync(string usuario)
        => await _context.Mensajes
            .Where(m => m.Destinatario == usuario && !m.EliminadoPorDestinatario)
            .OrderByDescending(m => m.FechaEnvio)
            .ToListAsync();

    public async Task<List<Mensaje>> GetEnviadosAsync(string usuario)
        => await _context.Mensajes
            .Where(m => m.Remitente == usuario && !m.EliminadoPorRemitente)
            .OrderByDescending(m => m.FechaEnvio)
            .ToListAsync();

    public async Task<Mensaje?> GetByIdAsync(Guid id)
        => await _context.Mensajes.FindAsync(id);

    public async Task<int> GetNoLeidosCountAsync(string usuario)
        => await _context.Mensajes
            .CountAsync(m => m.Destinatario == usuario && !m.Leido && !m.EliminadoPorDestinatario);

    public async Task AddAsync(Mensaje mensaje)
        => await _context.Mensajes.AddAsync(mensaje);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}