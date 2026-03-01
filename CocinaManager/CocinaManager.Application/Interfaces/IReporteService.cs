namespace CocinaManager.Application.Interfaces;

public interface IReporteService
{
    Task<byte[]> ExportarPersonalAsync();
    Task<byte[]> ExportarTurnosAsync();
    Task<byte[]> ExportarProductosStockAsync();
    Task<byte[]> ExportarMovimientosStockAsync();
}