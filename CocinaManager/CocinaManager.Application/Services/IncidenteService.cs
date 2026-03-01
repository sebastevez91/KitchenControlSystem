using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;

public class IncidenteService : IIncidenteService
{
    private readonly IIncidenteRepository _repo;
    private readonly ILogger<IncidenteService> _logger;

    public IncidenteService(IIncidenteRepository repo, ILogger<IncidenteService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<IncidenteDto>> GetAllAsync()
    {
        _logger.LogInformation("Consultando incidentes.");
        return (await _repo.GetAllAsync()).Select(Map).ToList();
    }

    public async Task<List<IncidenteDto>> GetByEstadoAsync(EstadoIncidente estado)
    {
        _logger.LogInformation("Consultando incidentes con estado {Estado}", estado);
        return (await _repo.GetByEstadoAsync(estado)).Select(Map).ToList();
    }

    public async Task<IncidenteDto> CreateAsync(CreateIncidenteDto dto, string registradoPor)
    {
        _logger.LogInformation("Registrando incidente tipo {Tipo} por {Usuario}", dto.Tipo, registradoPor);
        var incidente = new Incidente(dto.Descripcion, dto.Tipo, registradoPor);
        await _repo.AddAsync(incidente);
        await _repo.SaveChangesAsync();
        return Map(incidente);
    }

    public async Task<bool> ResolverAsync(Guid id, ResolverIncidenteDto dto)
    {
        _logger.LogInformation("Resolviendo incidente {Id}", id);
        var incidente = await _repo.GetByIdAsync(id);
        if (incidente == null) return false;
        incidente.Resolver(dto.Observaciones);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static IncidenteDto Map(Incidente i) => new()
    {
        Id = i.Id,
        Descripcion = i.Descripcion,
        Tipo = i.Tipo,
        Estado = i.Estado,
        FechaRegistro = i.FechaRegistro,
        FechaResolucion = i.FechaResolucion,
        RegistradoPor = i.RegistradoPor,
        Observaciones = i.Observaciones
    };
}