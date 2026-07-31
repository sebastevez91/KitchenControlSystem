using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;
public class MovimientoStockService : IMovimientoStockService
{
    private readonly IMovimientoStockRepository _movRepo;
    private readonly IProductoRepository _productoRepo;
    private readonly ILogger<MovimientoStockService> _logger;

    public MovimientoStockService(
        IMovimientoStockRepository movRepo,
        IProductoRepository productoRepo,
        ILogger<MovimientoStockService> logger)
    {
        _movRepo = movRepo;
        _productoRepo = productoRepo;
        _logger = logger;
    }

    public async Task<List<MovimientoStockDto>> GetAllAsync()
    {
        _logger.LogInformation("Consultando todos los movimientos de stock.");
        return (await _movRepo.GetAllAsync()).Select(Map).ToList();
    }

    public async Task<List<MovimientoStockDto>> GetByProductoIdAsync(Guid productoId)
    {
        _logger.LogInformation("Consultando movimientos del producto {ProductoId}", productoId);
        return (await _movRepo.GetByProductoIdAsync(productoId)).Select(Map).ToList();
    }

    public async Task<MovimientoStockDto> RegistrarMovimientoAsync(CreateMovimientoStockDto dto)
    {
        _logger.LogInformation("Registrando movimiento {Tipo} de {Cantidad} para producto {ProductoId}",
            dto.TipoMovimiento, dto.Cantidad, dto.ProductoId);

        var producto = await _productoRepo.GetByIdAsync(dto.ProductoId)
            ?? throw new Exception($"Producto {dto.ProductoId} no encontrado.");

        if (dto.TipoMovimiento == TipoMovimiento.Entrada)
            producto.AumentarStock(dto.Cantidad);
        else
            producto.DisminuirStock(dto.Cantidad);

        _logger.LogInformation("Stock actualizado: {Nombre}, actual: {Stock}", producto.Nombre, producto.StockActual);

        if (producto.StockActual <= producto.StockMinimo)
            _logger.LogWarning("⚠️ ALERTA: Producto '{Nombre}' con stock bajo. Actual: {Actual}, Mínimo: {Minimo}",
                producto.Nombre, producto.StockActual, producto.StockMinimo);

        var movimiento = new MovimientoStock(dto.ProductoId, dto.TipoMovimiento, dto.Cantidad, dto.Observacion, dto.Fecha);
        await _movRepo.AddAsync(movimiento);
        await _movRepo.SaveChangesAsync();

        return Map(movimiento);
    }

    public async Task<List<MovimientoStockDto>> GetByFechaAsync(DateTime fecha)
    {
        var lista = await _movRepo.GetByFechaAsync(fecha);
        return lista.Select(Map).ToList();
    }

    public async Task<List<MovimientoStockDto>> GetByFechaTipoAsync(DateTime fecha, TipoMovimiento tipo)
    {
        var lista = await _movRepo.GetByFechaTipoAsync(fecha, tipo);
        return lista.Select(Map).ToList();
    }

    /// <summary>
    /// Elimina un movimiento y ajusta el stock del producto revertiendo el movimiento.
    /// Devuelve true si la eliminación se realizó; false si se denegó por falta de stock o por edad del movimiento.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid movimientoId, TimeSpan? maxAge = null)
    {
        var movimiento = (await _movRepo.GetAllAsync()).FirstOrDefault(m => m.Id == movimientoId);
        if (movimiento == null)
        {
            _logger.LogWarning("Movimiento {Id} no encontrado.", movimientoId);
            return false;
        }

        var producto = await _productoRepo.GetByIdAsync(movimiento.ProductoId);
        if (producto == null)
        {
            _logger.LogError("Producto {Id} del movimiento {MovId} no encontrado.", movimiento.ProductoId, movimientoId);
            return false;
        }

        // Validación de antigüedad (por defecto 7 días)
        var limite = maxAge ?? TimeSpan.FromDays(7);
        if (DateTime.UtcNow - movimiento.Fecha > limite)
        {
            _logger.LogWarning("No se permite eliminar movimiento {Id}: excede el límite de {Dias} días.", movimientoId, limite.TotalDays);
            return false;
        }

        // Revertir el efecto en stock:
        if (movimiento.TipoMovimiento == TipoMovimiento.Entrada)
        {
            // Si eliminamos una entrada, debemos restar esa cantidad del stock actual.
            if (producto.StockActual < movimiento.Cantidad)
            {
                _logger.LogWarning("No se puede eliminar la entrada {Id}: producto {ProductoId} no tiene stock suficiente para revertir ({Stock} < {Cantidad}).",
                    movimientoId, producto.Id, producto.StockActual, movimiento.Cantidad);
                return false;
            }

            producto.DisminuirStock(movimiento.Cantidad);
        }
        else
        {
            // Si eliminamos una salida, revertimos aumentando el stock.
            producto.AumentarStock(movimiento.Cantidad);
        }

        await _movRepo.RemoveAsync(movimiento); // <-- usar método asíncrono del repositorio
        await _movRepo.SaveChangesAsync();

        _logger.LogInformation("Movimiento {Id} eliminado y stock ajustado para producto {ProductoId}.", movimientoId, producto.Id);
        return true;
    }

    private static MovimientoStockDto Map(MovimientoStock m) => new()
    {
        Id = m.Id,
        ProductoId = m.ProductoId,
        NombreProducto = m.Producto?.Nombre ?? string.Empty,
        TipoMovimiento = m.TipoMovimiento,
        Cantidad = m.Cantidad,
        Fecha = m.Fecha,
        Observacion = m.Observacion
    };
}