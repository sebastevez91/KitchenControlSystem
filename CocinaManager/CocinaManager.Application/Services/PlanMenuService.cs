using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;

public class PlanMenuService : IPlanMenuService
{
    private readonly IPlanMenuRepository _repo;
    private readonly ILogger<PlanMenuService> _logger;

    public PlanMenuService(IPlanMenuRepository repo, ILogger<PlanMenuService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<PlanMenuDto>> GetAllAsync()
    {
        var planes = await _repo.GetAllWithItemsAsync();
        return planes.Select(Map).ToList();
    }

    public async Task<PlanMenuDto?> GetByIdAsync(Guid id)
    {
        var p = await _repo.GetByIdWithItemsAsync(id);
        return p == null ? null : Map(p);
    }

    public async Task<PlanMenuDto> CreateAsync(CreatePlanMenuDto dto)
    {
        var plan = await _repo.GetByDiaYTipoAsync(dto.DiaSemana, dto.TipoMenu);
        if (plan == null)
        {
            plan = new PlanMenu(dto.DiaSemana, dto.TipoMenu);
            await _repo.AddAsync(plan);
        }

        foreach (var rid in dto.RecetaIds.Distinct())
            plan.AgregarReceta(rid);

        await _repo.SaveChangesAsync();
        return Map(plan);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var p = await _repo.GetByIdWithItemsAsync(id);
        if (p == null) return false;
        await _repo.RemoveAsync(p);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddRecetaAsync(Guid planId, Guid recetaId)
    {
        var plan = await _repo.GetByIdWithItemsAsync(planId);
        if (plan == null) return false;
        plan.AgregarReceta(recetaId);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveItemAsync(Guid planId, Guid itemId)
    {
        var plan = await _repo.GetByIdWithItemsAsync(planId);
        if (plan == null) return false;
        plan.RemoverItem(itemId);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task SetRecetaAsync(DiaSemana diaSemana, TipoMenu tipoMenu, Guid recetaId, RolPlato? rol = null)
    {
        var plan = await _repo.GetByDiaYTipoAsync(diaSemana, tipoMenu);
        if (plan == null)
        {
            plan = new PlanMenu(diaSemana, tipoMenu);
            await _repo.AddAsync(plan);
            await _repo.SaveChangesAsync();
        }
        await _repo.ReemplazarItemAsync(plan.Id, recetaId, rol);
    }

    public async Task LimpiarCeldaAsync(DiaSemana diaSemana, TipoMenu tipoMenu, RolPlato? rol = null)
    {
        var plan = await _repo.GetByDiaYTipoAsync(diaSemana, tipoMenu);
        if (plan == null) return;
        await _repo.LimpiarItemsAsync(plan.Id, rol);
    }

    private static PlanMenuDto Map(PlanMenu p) => new(
        p.Id,
        p.DiaSemana,
        p.TipoMenu,
        p.Items.Select(i => new PlanMenuItemDto(i.Id, i.RecetaId, i.Receta?.Nombre, i.Rol)).ToList()
    );
}