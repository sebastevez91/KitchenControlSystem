using CocinaManager.Application.DTOs;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Interfaces;

public interface IIncidenteService
{
    Task<List<IncidenteDto>> GetAllAsync();
    Task<List<IncidenteDto>> GetByEstadoAsync(EstadoIncidente estado);
    Task<IncidenteDto> CreateAsync(CreateIncidenteDto dto, string registradoPor);
    Task<bool> ResolverAsync(Guid id, ResolverIncidenteDto dto);
}