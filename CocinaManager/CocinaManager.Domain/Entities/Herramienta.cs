using CocinaManager.Domain.Enums;

namespace CocinaManager.Domain.Entities;

public class Herramienta
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string Descripcion { get; private set; }
    public EstadoHerramienta Estado { get; private set; }
    public DateTime FechaRegistro { get; private set; }
    public bool Activo { get; private set; }

    private Herramienta() { }

    public Herramienta(string nombre, string descripcion)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        Descripcion = descripcion;
        Estado = EstadoHerramienta.Operativa;
        FechaRegistro = DateTime.UtcNow;
        Activo = true;
    }

    public void CambiarEstado(EstadoHerramienta nuevoEstado)
    {
        Estado = nuevoEstado;
    }

    public void Desactivar()
    {
        Activo = false;
    }
}