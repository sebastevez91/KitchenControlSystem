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

        var movimiento = new MovimientoStock(dto.ProductoId, dto.TipoMovimiento, dto.Cantidad, dto.Observacion);
        await _movRepo.AddAsync(movimiento);
        await _movRepo.SaveChangesAsync();

        return Map(movimiento);
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