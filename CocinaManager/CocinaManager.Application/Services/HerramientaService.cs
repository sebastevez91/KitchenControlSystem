using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;

public class HerramientaService : IHerramientaService
{
    private readonly IHerramientaRepository _repo;
    private readonly ILogger<HerramientaService> _logger;

    public HerramientaService(IHerramientaRepository repo, ILogger<HerramientaService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<HerramientaDto>> GetAllAsync()
    {
        _logger.LogInformation("Consultando lista de herramientas.");
        return (await _repo.GetAllAsync()).Select(Map).ToList();
    }

    public async Task<HerramientaDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Buscando herramienta con Id: {Id}", id);
        var h = await _repo.GetByIdAsync(id);
        if (h == null) _logger.LogWarning("Herramienta con Id {Id} no encontrada.", id);
        return h == null ? null : Map(h);
    }

    public async Task<List<HerramientaDto>> GetByEstadoAsync(EstadoHerramienta estado)
    {
        _logger.LogInformation("Consultando herramientas con estado: {Estado}", estado);
        return (await _repo.GetByEstadoAsync(estado)).Select(Map).ToList();
    }

    public async Task<HerramientaDto> CreateAsync(CreateHerramientaDto dto)
    {
        _logger.LogInformation("Creando herramienta: {Nombre}", dto.Nombre);
        var herramienta = new Herramienta(dto.Nombre, dto.Descripcion);
        await _repo.AddAsync(herramienta);
        await _repo.SaveChangesAsync();
        _logger.LogInformation("Herramienta creada con Id: {Id}", herramienta.Id);
        return Map(herramienta);
    }

    public async Task<bool> CambiarEstadoAsync(Guid id, EstadoHerramienta nuevoEstado)
    {
        _logger.LogInformation("Cambiando estado de herramienta {Id} a {Estado}", id, nuevoEstado);
        var herramienta = await _repo.GetByIdAsync(id);
        if (herramienta == null)
        {
            _logger.LogWarning("Herramienta con Id {Id} no encontrada.", id);
            return false;
        }
        herramienta.CambiarEstado(nuevoEstado);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Desactivando herramienta con Id: {Id}", id);
        var herramienta = await _repo.GetByIdAsync(id);
        if (herramienta == null) return false;
        _repo.Remove(herramienta);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static HerramientaDto Map(Herramienta h) => new()
    {
        Id = h.Id,
        Nombre = h.Nombre,
        Descripcion = h.Descripcion,
        Estado = h.Estado,
        FechaRegistro = h.FechaRegistro
    };
}