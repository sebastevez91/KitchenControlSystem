using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IMovimientoStockService
{
    Task<List<MovimientoStockDto>> GetAllAsync();
    Task<List<MovimientoStockDto>> GetByProductoIdAsync(Guid productoId);
    Task<MovimientoStockDto> RegistrarMovimientoAsync(CreateMovimientoStockDto dto);
}