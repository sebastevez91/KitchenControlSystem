using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface ITurnoService
{
    Task<List<TurnoDto>> GetAllAsync();
    Task<List<TurnoDto>> GetByPersonalIdAsync(Guid personalId);
    Task<TurnoDto> CreateAsync(CreateTurnoDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<List<TurnoDto>> GetByFechaAsync(DateTime fecha);
    Task<List<TurnoDto>> GetByRangoAsync(DateTime inicio, DateTime fin);
}