using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using CocinaManager.Infrastructure.Data;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.Infrastructure.Repositories;

public class PlanMenuRepository : IPlanMenuRepository
{
    private readonly CocinaDbContext _db;
    public PlanMenuRepository(CocinaDbContext db) => _db = db;

    public async Task<List<PlanMenu>> GetAllWithItemsAsync() =>
        await _db.Set<PlanMenu>()
                  .Include(p => p.Items).ThenInclude(i => i.Receta)
                  .OrderBy(p => p.DiaSemana).ThenBy(p => p.TipoMenu)
                  .ToListAsync();

    public async Task<PlanMenu?> GetByIdWithItemsAsync(Guid id) =>
        await _db.Set<PlanMenu>()
                  .Include(p => p.Items).ThenInclude(i => i.Receta)
                  .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<PlanMenu?> GetByDiaYTipoAsync(DiaSemana diaSemana, TipoMenu tipoMenu) =>
        await _db.Set<PlanMenu>()
                  .Include(p => p.Items).ThenInclude(i => i.Receta)
                  .FirstOrDefaultAsync(p => p.DiaSemana == diaSemana && p.TipoMenu == tipoMenu);

    public Task AddAsync(PlanMenu plan) { _db.Add(plan); return Task.CompletedTask; }
    public Task RemoveAsync(PlanMenu plan) { _db.Remove(plan); return Task.CompletedTask; }
    public Task SaveChangesAsync() => _db.SaveChangesAsync();

    public async Task ReemplazarItemAsync(Guid planMenuId, Guid recetaId, RolPlato? rol)
    {
        var actuales = await _db.PlanMenuItems
            .Where(i => i.PlanMenuId == planMenuId && i.Rol == rol)
            .ToListAsync();

        _db.PlanMenuItems.RemoveRange(actuales);
        await _db.PlanMenuItems.AddAsync(new PlanMenuItem(planMenuId, recetaId, rol));
        await _db.SaveChangesAsync();
    }

    public async Task LimpiarItemsAsync(Guid planMenuId, RolPlato? rol)
    {
        var actuales = await _db.PlanMenuItems
            .Where(i => i.PlanMenuId == planMenuId && i.Rol == rol)
            .ToListAsync();

        _db.PlanMenuItems.RemoveRange(actuales);
        await _db.SaveChangesAsync();
    }
}