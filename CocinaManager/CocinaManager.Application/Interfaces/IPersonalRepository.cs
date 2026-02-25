using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IPersonalRepository
{
    Task<List<Personal>> GetAllAsync();
    Task<Personal?> GetByIdAsync(Guid id);
    Task AddAsync(Personal personal);
    void Remove(Personal personal);
    Task SaveChangesAsync();
}