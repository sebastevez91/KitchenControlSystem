using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IReporteService
{
    Task<byte[]> ExportarPersonalAsync();
    Task<byte[]> ExportarTurnosAsync();
    Task<byte[]> ExportarProductosStockAsync();
    Task<byte[]> ExportarMovimientosStockAsync();

    // Nuevo: calcula las cantidades de víveres necesarias para una fecha y cantidad de comensales,
    // y verifica disponibilidad en depósito.
    Task<ViveresResultadoDto> CalcularViveresAsync(DateTime fecha, int comensales = 35);
}