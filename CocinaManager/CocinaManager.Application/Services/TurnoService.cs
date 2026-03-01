using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;

public class TurnoService : ITurnoService
{
    private readonly ITurnoRepository _repo;
    private readonly ILogger<TurnoService> _logger;

    public TurnoService(ITurnoRepository repo, ILogger<TurnoService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<TurnoDto>> GetAllAsync()
    {
        _logger.LogInformation("Consultando lista de turnos.");
        return (await _repo.GetAllAsync()).Select(Map).ToList();
    }

    public async Task<List<TurnoDto>> GetByPersonalIdAsync(Guid personalId)
    {
        _logger.LogInformation("Consultando turnos del personal {PersonalId}", personalId);
        return (await _repo.GetByPersonalIdAsync(personalId)).Select(Map).ToList();
    }

    public async Task<TurnoDto> CreateAsync(CreateTurnoDto dto)
    {
        _logger.LogInformation("Creando turno para personal {PersonalId}", dto.PersonalId);
        var turno = new Turno(dto.Fecha, dto.TipoTurno, dto.PersonalId);
        await _repo.AddAsync(turno);
        await _repo.SaveChangesAsync();
        return Map(turno);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Eliminando turno {Id}", id);
        var turno = await _repo.GetByIdAsync(id);
        if (turno == null) return false;
        _repo.Remove(turno);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static TurnoDto Map(Turno t) => new()
    {
        Id = t.Id,
        Fecha = t.Fecha,
        TipoTurno = t.TipoTurno,
        PersonalId = t.PersonalId,
        NombrePersonal = t.Personal?.Nombre ?? string.Empty
    };

    public async Task<List<TurnoDto>> GetByFechaAsync(DateTime fecha)
    {
        _logger.LogInformation("Consultando turnos del día {Fecha}", fecha.ToShortDateString());
        return (await _repo.GetByFechaAsync(fecha)).Select(Map).ToList();
    }

    public async Task<List<TurnoDto>> GetByRangoAsync(DateTime inicio, DateTime fin)
    {
        _logger.LogInformation("Consultando turnos del {Inicio} al {Fin}", inicio.ToShortDateString(), fin.ToShortDateString());
        return (await _repo.GetByRangoAsync(inicio, fin)).Select(Map).ToList();
    }
}