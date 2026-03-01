using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;

public class PersonalService : IPersonalService
{
    private readonly IPersonalRepository _repo;
    private readonly ILogger<PersonalService> _logger;

    public PersonalService(IPersonalRepository repo, ILogger<PersonalService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<PersonalDto>> GetAllAsync()
    {
        _logger.LogInformation("Consultando lista de personal.");
        var lista = await _repo.GetAllAsync();
        return lista.Select(p => new PersonalDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Documento = p.Documento,
            Cargo = p.Cargo,
            Estado = p.Estado
        }).ToList();
    }

    public async Task<PersonalDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Buscando personal con Id: {Id}", id);
        var p = await _repo.GetByIdAsync(id);
        if (p == null)
        {
            _logger.LogWarning("Personal con Id {Id} no encontrado.", id);
            return null;
        }
        return new PersonalDto { Id = p.Id, Nombre = p.Nombre, Documento = p.Documento, Cargo = p.Cargo, Estado = p.Estado };
    }

    public async Task<PersonalDto> CreateAsync(CreatePersonalDto dto)
    {
        _logger.LogInformation("Creando personal: {Nombre}, Documento: {Documento}", dto.Nombre, dto.Documento);
        var personal = new Personal(dto.Nombre, dto.Documento, dto.Cargo);
        await _repo.AddAsync(personal);
        await _repo.SaveChangesAsync();
        _logger.LogInformation("Personal creado con Id: {Id}", personal.Id);
        return new PersonalDto { Id = personal.Id, Nombre = personal.Nombre, Documento = personal.Documento, Cargo = personal.Cargo, Estado = personal.Estado };
    }

    public async Task<bool> CambiarEstadoAsync(Guid id, EstadoPersonal nuevoEstado)
    {
        _logger.LogInformation("Cambiando estado de personal {Id} a {Estado}", id, nuevoEstado);
        var personal = await _repo.GetByIdAsync(id);
        if (personal == null)
        {
            _logger.LogWarning("Personal con Id {Id} no encontrado para cambiar estado.", id);
            return false;
        }
        personal.CambiarEstado(nuevoEstado);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Eliminando personal con Id: {Id}", id);
        var personal = await _repo.GetByIdAsync(id);
        if (personal == null)
        {
            _logger.LogWarning("Personal con Id {Id} no encontrado para eliminar.", id);
            return false;
        }
        _repo.Remove(personal);
        await _repo.SaveChangesAsync();
        _logger.LogInformation("Personal {Id} eliminado correctamente.", id);
        return true;
    }
}