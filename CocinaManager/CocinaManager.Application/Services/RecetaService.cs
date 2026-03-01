using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;

public class RecetaService : IRecetaService
{
    private readonly IRecetaRepository _repo;
    private readonly ILogger<RecetaService> _logger;

    public RecetaService(IRecetaRepository repo, ILogger<RecetaService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<RecetaDto>> GetAllAsync()
    {
        _logger.LogInformation("Consultando recetas.");
        return (await _repo.GetAllAsync()).Select(Map).ToList();
    }

    public async Task<RecetaDto?> GetByIdAsync(Guid id)
    {
        var r = await _repo.GetByIdAsync(id);
        return r == null ? null : Map(r);
    }

    public async Task<RecetaDto> CreateAsync(CreateRecetaDto dto)
    {
        _logger.LogInformation("Creando receta: {Nombre}", dto.Nombre);
        var receta = new Receta(dto.Nombre, dto.Descripcion, dto.Porciones);

        foreach (var i in dto.Ingredientes)
            receta.Ingredientes.Add(
                new RecetaIngrediente(receta.Id, i.Nombre, i.Cantidad, i.UnidadMedida));

        await _repo.AddAsync(receta);
        await _repo.SaveChangesAsync();
        return Map(receta);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var receta = await _repo.GetByIdAsync(id);
        if (receta == null) return false;
        _repo.Remove(receta);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static RecetaDto Map(Receta r) => new()
    {
        Id = r.Id,
        Nombre = r.Nombre,
        Descripcion = r.Descripcion,
        Porciones = r.Porciones,
        FechaCreacion = r.FechaCreacion,
        Ingredientes = r.Ingredientes.Select(i => new RecetaIngredienteDto
        {
            Id = i.Id,
            Nombre = i.Nombre,
            Cantidad = i.Cantidad,
            UnidadMedida = i.UnidadMedida
        }).ToList()
    };
}