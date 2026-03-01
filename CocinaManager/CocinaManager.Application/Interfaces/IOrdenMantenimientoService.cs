using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IOrdenMantenimientoService
{
    Task<List<OrdenMantenimientoDto>> GetAllAsync();
    Task<List<OrdenMantenimientoDto>> GetByHerramientaIdAsync(Guid herramientaId);
    Task<OrdenMantenimientoDto> CreateAsync(CreateOrdenMantenimientoDto dto);
    Task<bool> ResolverAsync(Guid id, ResolverOrdenDto dto);
    Task<bool> CancelarAsync(Guid id, ResolverOrdenDto dto);
    Task<List<OrdenMantenimientoDto>> GetPendientesAsync();
}