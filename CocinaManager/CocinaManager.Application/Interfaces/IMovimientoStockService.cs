using CocinaManager.Application.DTOs;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Interfaces;

public interface IMovimientoStockService
{
    Task<List<MovimientoStockDto>> GetAllAsync();
    Task<List<MovimientoStockDto>> GetByProductoIdAsync(Guid productoId);
    Task<MovimientoStockDto> RegistrarMovimientoAsync(CreateMovimientoStockDto dto);

    // Nuevo: obtener movimientos de un día completo
    Task<List<MovimientoStockDto>> GetByFechaAsync(DateTime fecha);

    // Nuevo: obtener movimientos de un día y por tipo (Entrada o Salida)
    Task<List<MovimientoStockDto>> GetByFechaTipoAsync(DateTime fecha, TipoMovimiento tipo);

    // Nuevo: eliminar/deshacer un movimiento (devuelve true si se eliminó)
    Task<bool> DeleteAsync(Guid movimientoId, TimeSpan? maxAge = null);
}