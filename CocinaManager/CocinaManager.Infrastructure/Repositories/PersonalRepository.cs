using Microsoft.EntityFrameworkCore;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;

namespace CocinaManager.Infrastructure.Repositories;

public class PersonalRepository : IPersonalRepository
{
    private readonly CocinaDbContext _context;

    public PersonalRepository(CocinaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Personal>> GetAllAsync()
        => await _context.Personal.ToListAsync();

    public async Task<Personal?> GetByIdAsync(Guid id)
        => await _context.Personal.FindAsync(id);

    public async Task AddAsync(Personal personal)
        => await _context.Personal.AddAsync(personal);

    public void Remove(Personal personal)
        => _context.Personal.Remove(personal);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}