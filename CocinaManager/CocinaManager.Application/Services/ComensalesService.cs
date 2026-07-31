using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Services;

public class ComensalesService : IComensalesService
{
    private readonly IRegistroComensalesRepository _repo;
    public ComensalesService(IRegistroComensalesRepository repo) => _repo = repo;

    public async Task<int?> GetCantidadAsync(DateTime fecha) =>
        (await _repo.GetByFechaAsync(fecha))?.Cantidad;

    public async Task RegistrarAsync(DateTime fecha, int cantidad)
    {
        if (cantidad < 0) throw new ArgumentException("La cantidad no puede ser negativa.");
        var existente = await _repo.GetByFechaAsync(fecha);
        if (existente != null) existente.ActualizarCantidad(cantidad);
        else await _repo.AddAsync(new RegistroComensales(fecha, cantidad));
        await _repo.SaveChangesAsync();
    }
}
