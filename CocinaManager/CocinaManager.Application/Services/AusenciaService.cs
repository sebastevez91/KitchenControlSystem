using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;

public class AusenciaService : IAusenciaService
{
    private readonly IAusenciaRepository _repo;
    private readonly ILogger<AusenciaService> _logger;

    public AusenciaService(IAusenciaRepository repo, ILogger<AusenciaService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<AusenciaDto>> GetAllAsync()
    {
        _logger.LogInformation("Consultando ausencias.");
        return (await _repo.GetAllAsync()).Select(Map).ToList();
    }

    public async Task<List<AusenciaDto>> GetByPersonalIdAsync(Guid personalId)
    {
        _logger.LogInformation("Consultando ausencias del personal {Id}", personalId);
        return (await _repo.GetByPersonalIdAsync(personalId)).Select(Map).ToList();
    }

    public async Task<AusenciaDto> CreateAsync(CreateAusenciaDto dto, string registradoPor)
    {
        _logger.LogInformation("Registrando ausencia tipo {Tipo} para personal {Id}", dto.Tipo, dto.PersonalId);
        var ausencia = new Ausencia(dto.PersonalId, dto.Tipo, dto.FechaInicio, dto.FechaFin, registradoPor);
        await _repo.AddAsync(ausencia);
        await _repo.SaveChangesAsync();
        return Map(ausencia);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var ausencia = await _repo.GetByIdAsync(id);
        if (ausencia == null) return false;
        _repo.Remove(ausencia);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static AusenciaDto Map(Ausencia a) => new()
    {
        Id = a.Id,
        PersonalId = a.PersonalId,
        NombrePersonal = a.Personal?.Nombre ?? string.Empty,
        Tipo = a.Tipo,
        FechaInicio = a.FechaInicio,
        FechaFin = a.FechaFin,
        DiasAusencia = a.DiasAusencia,
        RegistradoPor = a.RegistradoPor,
        FechaRegistro = a.FechaRegistro
    };
}