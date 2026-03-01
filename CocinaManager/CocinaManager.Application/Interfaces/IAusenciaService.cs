using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IAusenciaService
{
    Task<List<AusenciaDto>> GetAllAsync();
    Task<List<AusenciaDto>> GetByPersonalIdAsync(Guid personalId);
    Task<AusenciaDto> CreateAsync(CreateAusenciaDto dto, string registradoPor);
    Task<bool> DeleteAsync(Guid id);
}