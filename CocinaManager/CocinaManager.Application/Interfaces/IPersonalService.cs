using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IPersonalService
{
    Task<List<PersonalDto>> GetAllAsync();
    Task<PersonalDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreatePersonalDto dto);
    Task<bool> DeleteAsync(Guid id);
}