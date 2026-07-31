using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IRecepcionViveresService
{
    Task<RecepcionViveresDto> CreateAsync(CreateRecepcionViveresDto dto);
    Task<RecepcionViveresDto?> GetByIdAsync(Guid id);
    Task<List<RecepcionViveresDto>> GetByFechaAsync(DateTime fecha);
    Task<bool> DeleteAsync(Guid id);
}