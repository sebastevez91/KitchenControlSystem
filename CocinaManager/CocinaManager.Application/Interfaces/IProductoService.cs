using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IProductoService
{
    Task<List<ProductoDto>> GetAllAsync();
    Task<ProductoDto?> GetByIdAsync(Guid id);
    Task<List<ProductoDto>> GetStockBajoAsync();
    Task<ProductoDto> CreateAsync(CreateProductoDto dto);
    Task<bool> DeleteAsync(Guid id);
}