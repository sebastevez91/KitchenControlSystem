using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.Infrastructure.Repositories;

public class RegistroComensalesRepository : IRegistroComensalesRepository
{
    private readonly CocinaDbContext _db;
    public RegistroComensalesRepository(CocinaDbContext db) => _db = db;

    public Task<RegistroComensales?> GetByFechaAsync(DateTime fecha) =>
        _db.RegistrosComensales.FirstOrDefaultAsync(r => r.Fecha == fecha.Date);

    public Task<List<RegistroComensales>> GetByFechasAsync(IEnumerable<DateTime> fechas)
    {
        var f = fechas.Select(x => x.Date).ToList();
        return _db.RegistrosComensales.Where(r => f.Contains(r.Fecha)).ToListAsync();
    }

    public Task AddAsync(RegistroComensales r) { _db.Add(r); return Task.CompletedTask; }
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
