using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Services;

public class RecepcionViveresService : IRecepcionViveresService
{
    private readonly IRecepcionViveresRepository _repo;
    private readonly IMovimientoStockService _movimientoService;

    public RecepcionViveresService(IRecepcionViveresRepository repo, IMovimientoStockService movimientoService)
    {
        _repo = repo;
        _movimientoService = movimientoService;
    }

    public async Task<RecepcionViveresDto> CreateAsync(CreateRecepcionViveresDto dto)
    {
        var recepcion = new RecepcionViveres(dto.Fecha, dto.Proveedor, dto.Remito, dto.RecibidoPor, dto.DniRecibidoPor, dto.Observaciones);
        foreach (var l in dto.Lineas)
            recepcion.AddLinea(l.ProductoId, l.NombreProducto ?? string.Empty, l.Cantidad, l.Unidad, l.Observacion);

        await _repo.AddAsync(recepcion);
        await _repo.SaveChangesAsync();

        // Registrar movimientos de entrada para cada línea que tenga ProductoId válido
        foreach (var l in dto.Lineas)
        {
            if (l.ProductoId != Guid.Empty && l.Cantidad > 0)
            {
                var movDto = new CreateMovimientoStockDto
                {
                    ProductoId = l.ProductoId,
                    TipoMovimiento = TipoMovimiento.Entrada,
                    Cantidad = l.Cantidad,
                    Observacion = l.Observacion,
                    Fecha = dto.Fecha
                };
                // RegistrarMovimientoAsync actualiza stock y guarda cambios
                await _movimientoService.RegistrarMovimientoAsync(movDto);
            }
        }

        return Map(recepcion);
    }

    public async Task<RecepcionViveresDto?> GetByIdAsync(Guid id)
    {
        var r = await _repo.GetByIdAsync(id);
        return r == null ? null : Map(r);
    }

    public async Task<List<RecepcionViveresDto>> GetByFechaAsync(DateTime fecha)
    {
        var list = await _repo.GetByFechaAsync(fecha);
        return list.Select(Map).ToList();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var recepcion = await _repo.GetByIdAsync(id);
        if (recepcion == null) return false;

        // Para deshacer la recepción, registramos movimientos de salida por cada línea
        foreach (var l in recepcion.Lineas)
        {
            if (l.ProductoId != Guid.Empty && l.Cantidad > 0)
            {
                var movDto = new CreateMovimientoStockDto
                {
                    ProductoId = l.ProductoId,
                    TipoMovimiento = TipoMovimiento.Salida,
                    Cantidad = l.Cantidad,
                    Observacion = $"Deshacer recepción {recepcion.Id}",
                    Fecha = DateTime.UtcNow
                };
                await _movimientoService.RegistrarMovimientoAsync(movDto);
            }
        }

        await _repo.DeleteAsync(id);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static RecepcionViveresDto Map(RecepcionViveres r) => new()
    {
        Id = r.Id,
        Fecha = r.Fecha,
        Proveedor = r.Proveedor,
        Remito = r.Remito,
        RecibidoPor = r.RecibidoPor,
        DniRecibidoPor = r.DniRecibidoPor,
        Observaciones = r.Observaciones,
        Lineas = r.Lineas.Select(l => new RecepcionViveresLineaDto
        {
            ProductoId = l.ProductoId,
            NombreProducto = l.NombreProducto,
            Cantidad = l.Cantidad,
            Unidad = l.Unidad,
            Observacion = l.Observacion
        }).ToList()
    };
}