using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Interfaces;

public interface IMovimientoStockRepository
{
    Task<List<MovimientoStock>> GetAllAsync();
    Task<List<MovimientoStock>> GetByProductoIdAsync(Guid productoId);

    // Nuevo: obtener movimientos en una fecha (intervalo día)
    Task<List<MovimientoStock>> GetByFechaAsync(DateTime fecha);

    // Nuevo: obtener movimientos por fecha y tipo (Entrada/Salida)
    Task<List<MovimientoStock>> GetByFechaTipoAsync(DateTime fecha, TipoMovimiento tipo);

    Task AddAsync(MovimientoStock movimiento);
    Task RemoveAsync(MovimientoStock movimiento); // <-- agregado
    Task SaveChangesAsync();
}