using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using DocumentFormat.OpenXml.InkML;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;

public class OrdenMantenimientoService : IOrdenMantenimientoService
{
    private readonly IOrdenMantenimientoRepository _repo;
    private readonly IHerramientaRepository _herramientaRepo;
    private readonly ILogger<OrdenMantenimientoService> _logger;

    public OrdenMantenimientoService(
        IOrdenMantenimientoRepository repo,
        IHerramientaRepository herramientaRepo,
        ILogger<OrdenMantenimientoService> logger)
    {
        _repo = repo;
        _herramientaRepo = herramientaRepo;
        _logger = logger;
    }

    public async Task<List<OrdenMantenimientoDto>> GetAllAsync()
    {
        _logger.LogInformation("Consultando todas las órdenes de mantenimiento.");
        return (await _repo.GetAllAsync()).Select(Map).ToList();
    }

    public async Task<List<OrdenMantenimientoDto>> GetByHerramientaIdAsync(Guid herramientaId)
    {
        _logger.LogInformation("Consultando órdenes de herramienta {HerramientaId}", herramientaId);
        return (await _repo.GetByHerramientaIdAsync(herramientaId)).Select(Map).ToList();
    }

    public async Task<OrdenMantenimientoDto> CreateAsync(CreateOrdenMantenimientoDto dto)
    {
        _logger.LogInformation("Creando orden de mantenimiento para herramienta {HerramientaId}", dto.HerramientaId);

        var herramienta = await _herramientaRepo.GetByIdAsync(dto.HerramientaId)
            ?? throw new KeyNotFoundException($"Herramienta {dto.HerramientaId} no encontrada.");

        // Cambiar estado de la herramienta automáticamente
        herramienta.CambiarEstado(Domain.Enums.EstadoHerramienta.EnReparacion);

        var orden = new OrdenMantenimiento(dto.HerramientaId, dto.Descripcion);
        await _repo.AddAsync(orden);
        await _repo.SaveChangesAsync();

        _logger.LogInformation("Orden {Id} creada. Herramienta '{Nombre}' pasó a EnReparacion.", orden.Id, herramienta.Nombre);
        return Map(orden);
    }

    public async Task<bool> ResolverAsync(Guid id, ResolverOrdenDto dto)
    {
        _logger.LogInformation("Resolviendo orden {Id}", id);
        var orden = await _repo.GetByIdAsync(id);
        if (orden == null) return false;

        orden.Resolver(dto.Observaciones ?? string.Empty);

        // Devolver herramienta a Operativa
        var herramienta = await _herramientaRepo.GetByIdAsync(orden.HerramientaId);
        herramienta?.CambiarEstado(Domain.Enums.EstadoHerramienta.Operativa);

        await _repo.SaveChangesAsync();
        _logger.LogInformation("Orden {Id} resuelta. Herramienta devuelta a Operativa.", id);
        return true;
    }

    public async Task<bool> CancelarAsync(Guid id, ResolverOrdenDto dto)
    {
        _logger.LogInformation("Cancelando orden {Id}", id);
        var orden = await _repo.GetByIdAsync(id);
        if (orden == null) return false;

        orden.Cancelar(dto.Observaciones ?? string.Empty);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static OrdenMantenimientoDto Map(OrdenMantenimiento o) => new()
    {
        Id = o.Id,
        HerramientaId = o.HerramientaId,
        NombreHerramienta = o.Herramienta?.Nombre ?? string.Empty,
        Descripcion = o.Descripcion,
        EstadoOrden = o.EstadoOrden,
        FechaCreacion = o.FechaCreacion,
        FechaResolucion = o.FechaResolucion,
        Observaciones = o.Observaciones
    };

    public async Task<List<OrdenMantenimientoDto>> GetPendientesAsync()
    {
        _logger.LogInformation("Consultando órdenes pendientes.");
        return (await _repo.GetPendientesAsync()).Select(Map).ToList();
    }

}