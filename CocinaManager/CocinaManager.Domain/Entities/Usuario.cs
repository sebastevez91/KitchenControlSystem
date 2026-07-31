using CocinaManager.Domain.Enums;

namespace CocinaManager.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string NombreUsuario { get; private set; }
    public string PasswordHash { get; private set; }
    public RolUsuario Rol { get; private set; }
    public string? Nombre { get; private set; }
    public string? Apellido { get; private set; }
    public bool Activo { get; private set; }

    private Usuario() { }

    // Sobrecarga para mantener compatibilidad con llamadas que no proveían nombre/apellido
    public Usuario(string nombreUsuario, string passwordHash, RolUsuario rol)
        : this(nombreUsuario, passwordHash, rol, null, null)
    {
    }

    public Usuario(string nombreUsuario, string passwordHash, RolUsuario rol, string? nombre, string? apellido)
    {
        Id = Guid.NewGuid();
        NombreUsuario = nombreUsuario;
        PasswordHash = passwordHash;
        Rol = rol;
        Nombre = nombre;
        Apellido = apellido;
        Activo = true;
    }

    public void CambiarPassword(string nuevoHash)
    {
        PasswordHash = nuevoHash;
    }

    // Nuevo método para actualizar nombre y apellido desde el backend
    public void ActualizarNombreApellido(string? nombre, string? apellido)
    {
        Nombre = string.IsNullOrWhiteSpace(nombre) ? null : nombre;
        Apellido = string.IsNullOrWhiteSpace(apellido) ? null : apellido;
    }
}