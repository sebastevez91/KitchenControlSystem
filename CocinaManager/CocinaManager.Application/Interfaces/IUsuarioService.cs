namespace CocinaManager.Application.Interfaces;

public interface IUsuarioService
{
    Task<(bool success, string rol)> ValidarCredencialesAsync(string nombreUsuario, string password);
}