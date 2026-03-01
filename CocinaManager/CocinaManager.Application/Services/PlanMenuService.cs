using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
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

    public async Task<List<PlanMenuDto>> GetByRangoAsync(DateTime inicio, DateTime fin)
        => (await _repo.GetByRangoAsync(inicio, fin)).Select(Map).ToList();

    public async Task<List<PlanMenuDto>> GetByFechaAsync(DateTime fecha)
        => (await _repo.GetByFechaAsync(fecha)).Select(Map).ToList();

    public async Task<PlanMenuDto> CreateAsync(CreatePlanMenuDto dto)
    {
        _logger.LogInformation("Planificando menú para {Fecha}", dto.Fecha.ToShortDateString());
        var plan = new PlanMenu(dto.Fecha, dto.RecetaId, dto.TipoComida, dto.Observaciones);
        await _repo.AddAsync(plan);
        await _repo.SaveChangesAsync();
        return Map(plan);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var plan = await _repo.GetByIdAsync(id);
        if (plan == null) return false;
        _repo.Remove(plan);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static PlanMenuDto Map(PlanMenu p) => new()
    {
        Id = p.Id,
        Fecha = p.Fecha,
        RecetaId = p.RecetaId,
        NombreReceta = p.Receta?.Nombre ?? string.Empty,
        TipoComida = p.TipoComida,
        Observaciones = p.Observaciones
    };
}