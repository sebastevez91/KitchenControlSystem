using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IMensajeRepository
{
    Task<List<Mensaje>> GetRecibidosAsync(string usuario);
    Task<List<Mensaje>> GetEnviadosAsync(string usuario);
    Task<Mensaje?> GetByIdAsync(Guid id);
    Task<int> GetNoLeidosCountAsync(string usuario);
    Task AddAsync(Mensaje mensaje);
    Task SaveChangesAsync();
}