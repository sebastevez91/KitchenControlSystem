using CocinaManager.Domain.Entities;

namespace CocinaManager.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByCredencialesAsync(string nombreUsuario, string passwordHash);
}