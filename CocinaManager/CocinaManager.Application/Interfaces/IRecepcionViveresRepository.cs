using CocinaManager.Application.DTOs;
using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IRecepcionViveresRepository
{
    Task AddAsync(RecepcionViveres recepcion);
    Task<RecepcionViveres?> GetByIdAsync(Guid id);
    Task<List<RecepcionViveres>> GetByFechaAsync(DateTime fecha);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
    Task<byte[]> GeneratePdfFromDtoAsync(RecepcionViveresDto dto);
}