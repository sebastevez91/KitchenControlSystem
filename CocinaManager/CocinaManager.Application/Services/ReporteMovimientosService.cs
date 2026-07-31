using System;
using System.Linq;
using System.Threading.Tasks;
using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Services;

public class ReporteMovimientosService : IRecepcionMovimientosService
{
    private readonly IMovimientoStockService _movimientoService;
    private readonly IRecepcionViveresRepository _recepcionRepo;

    public ReporteMovimientosService(IMovimientoStockService movimientoService, IRecepcionViveresRepository recepcionRepo)
    {
        _movimientoService = movimientoService;
        _recepcionRepo = recepcionRepo;
    }

    public async Task<byte[]> GeneratePdfEntradasPorFechaAsync(DateTime fecha, string? proveedor = null, string? remito = null, string? recibidoPor = null, string? dni = null, string? observaciones = null)
    {
        // Obtener movimientos de tipo Entrada del día (no modifica la base)
        var movimientos = await _movimientoService.GetByFechaTipoAsync(fecha, TipoMovimiento.Entrada);

        // Mapear a DTO de recepción (draft, sin persistir)
        var dto = new RecepcionViveresDto
        {
            Id = Guid.Empty,
            Fecha = fecha,
            Proveedor = proveedor,
            Remito = remito,
            RecibidoPor = recibidoPor,
            DniRecibidoPor = dni,
            Observaciones = observaciones,
            Lineas = movimientos.Select(m => new RecepcionViveresLineaDto
            {
                MovimientoStockId = m.Id,
                ProductoId = m.ProductoId,
                NombreProducto = m.NombreProducto ?? string.Empty,
                Cantidad = m.Cantidad,
                Unidad = null,
                Observacion = m.Observacion
            }).ToList()
        };

        // Reutiliza el generador de PDFs del repositorio (no guarda nada)
        return await _recepcionRepo.GeneratePdfFromDtoAsync(dto);
    }
}