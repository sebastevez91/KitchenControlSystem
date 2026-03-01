using CocinaManager.Application.DTOs;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Interfaces;

public interface IPersonalService
{
    Task<List<PersonalDto>> GetAllAsync();
    Task<PersonalDto?> GetByIdAsync(Guid id);
    Task<PersonalDto> CreateAsync(CreatePersonalDto dto);
    Task<bool> CambiarEstadoAsync(Guid id, EstadoPersonal nuevoEstado);
    Task<bool> DeleteAsync(Guid id);
}