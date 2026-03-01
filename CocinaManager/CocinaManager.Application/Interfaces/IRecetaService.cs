using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IRecetaService
{
    Task<List<RecetaDto>> GetAllAsync();
    Task<RecetaDto?> GetByIdAsync(Guid id);
    Task<RecetaDto> CreateAsync(CreateRecetaDto dto);
    Task<bool> DeleteAsync(Guid id);
}