using CocinaManager.Application.DTOs;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Interfaces;

public interface IHerramientaService
{
    Task<List<HerramientaDto>> GetAllAsync();
    Task<HerramientaDto?> GetByIdAsync(Guid id);
    Task<List<HerramientaDto>> GetByEstadoAsync(EstadoHerramienta estado);
    Task<HerramientaDto> CreateAsync(CreateHerramientaDto dto);
    Task<bool> CambiarEstadoAsync(Guid id, EstadoHerramienta nuevoEstado);
    Task<bool> DeleteAsync(Guid id);
}