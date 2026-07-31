using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IRecetaService
{
    Task<List<RecetaDto>> GetAllAsync(bool incluirInactivas = false);
    Task<RecetaDto?> GetByIdAsync(Guid id);
    Task<RecetaDto> CreateAsync(CreateRecetaDto dto);
    Task<RecetaDto> UpdateAsync(UpdateRecetaDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ToggleAcitvoAsync(Guid id);
}