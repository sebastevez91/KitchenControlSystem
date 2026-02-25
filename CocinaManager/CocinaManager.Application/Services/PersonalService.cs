using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Services;

public class PersonalService : IPersonalService
{
    private readonly IPersonalRepository _repository;

    public PersonalService(IPersonalRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PersonalDto>> GetAllAsync()
    {
        var personal = await _repository.GetAllAsync();

        return personal.Select(p => new PersonalDto
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
        var p = await _repository.GetByIdAsync(id);

        if (p == null)
            return null;

        return new PersonalDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Documento = p.Documento,
            Cargo = p.Cargo,
            Estado = p.Estado
        };
    }

    public async Task<Guid> CreateAsync(CreatePersonalDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new ArgumentException("El nombre es obligatorio");

        if (string.IsNullOrWhiteSpace(dto.Documento))
            throw new ArgumentException("El documento es obligatorio");

        var personal = new Personal(dto.Nombre, dto.Documento, dto.Cargo);

        await _repository.AddAsync(personal);
        await _repository.SaveChangesAsync();

        return personal.Id;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var personal = await _repository.GetByIdAsync(id);

        if (personal == null)
            return false;

        _repository.Remove(personal);
        await _repository.SaveChangesAsync();

        return true;
    }
}