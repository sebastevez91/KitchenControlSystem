using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repo;
    private readonly ILogger<ProductoService> _logger;

    public ProductoService(IProductoRepository repo, ILogger<ProductoService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<ProductoDto>> GetAllAsync()
    {
        _logger.LogInformation("Consultando lista de productos.");
        return (await _repo.GetAllAsync()).Select(Map).ToList();
    }

    public async Task<ProductoDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Buscando producto con Id: {Id}", id);
        var p = await _repo.GetByIdAsync(id);
        if (p == null) _logger.LogWarning("Producto con Id {Id} no encontrado.", id);
        return p == null ? null : Map(p);
    }

    public async Task<List<ProductoDto>> GetStockBajoAsync()
    {
        _logger.LogInformation("Consultando productos con stock bajo.");
        return (await _repo.GetStockBajoAsync()).Select(Map).ToList();
    }

    public async Task<ProductoDto> CreateAsync(CreateProductoDto dto)
    {
        _logger.LogInformation("Creando producto: {Nombre}", dto.Nombre);
        var producto = new Producto(dto.Nombre, dto.UnidadMedida, dto.StockMinimo, dto.CategoriaId);
        await _repo.AddAsync(producto);
        await _repo.SaveChangesAsync();
        _logger.LogInformation("Producto creado con Id: {Id}", producto.Id);
        return Map(producto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Eliminando producto con Id: {Id}", id);
        var producto = await _repo.GetByIdAsync(id);
        if (producto == null)
        {
            _logger.LogWarning("Producto con Id {Id} no encontrado.", id);
            return false;
        }
        _repo.Remove(producto);
        await _repo.SaveChangesAsync();
        _logger.LogInformation("Producto {Id} eliminado.", id);
        return true;
    }

    private static ProductoDto Map(Producto p) => new()
    {
        Id = p.Id,
        Nombre = p.Nombre,
        UnidadMedida = p.UnidadMedida,
        StockActual = p.StockActual,
        StockMinimo = p.StockMinimo,
        CategoriaId = p.CategoriaId
    };
}