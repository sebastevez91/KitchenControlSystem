using CocinaManager.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CocinaManager.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;

    public UsuarioService(IUsuarioRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool success, string rol)> ValidarCredencialesAsync(string nombreUsuario, string password)
    {
        var hash = HashPassword(password);
        var usuario = await _repo.GetByCredencialesAsync(nombreUsuario, hash);

        if (usuario == null) return (false, string.Empty);
        return (true, usuario.Rol.ToString());
    }

    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}