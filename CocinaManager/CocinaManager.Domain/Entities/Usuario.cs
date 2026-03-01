using CocinaManager.Domain.Enums;

namespace CocinaManager.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string NombreUsuario { get; private set; }
    public string PasswordHash { get; private set; }
    public RolUsuario Rol { get; private set; }
    public bool Activo { get; private set; }

    private Usuario() { }

    public Usuario(string nombreUsuario, string passwordHash, RolUsuario rol)
    {
        Id = Guid.NewGuid();
        NombreUsuario = nombreUsuario;
        PasswordHash = passwordHash;
        Rol = rol;
        Activo = true;
    }

    public void CambiarPassword(string nuevoHash)
    {
        PasswordHash = nuevoHash;
    }
}