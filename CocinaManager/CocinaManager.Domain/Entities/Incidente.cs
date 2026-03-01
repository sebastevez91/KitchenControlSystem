using CocinaManager.Domain.Enums;

namespace CocinaManager.Domain.Entities;

public class Incidente
{
    public Guid Id { get; private set; }
    public string Descripcion { get; private set; }
    public TipoIncidente Tipo { get; private set; }
    public EstadoIncidente Estado { get; private set; }
    public DateTime FechaRegistro { get; private set; }
    public DateTime? FechaResolucion { get; private set; }
    public string RegistradoPor { get; private set; }
    public string? Observaciones { get; private set; }

    private Incidente() { }

    public Incidente(string descripcion, TipoIncidente tipo, string registradoPor)
    {
        Id = Guid.NewGuid();
        Descripcion = descripcion;
        Tipo = tipo;
        Estado = EstadoIncidente.Abierto;
        FechaRegistro = DateTime.UtcNow;
        RegistradoPor = registradoPor;
    }

    public void Resolver(string? observaciones)
    {
        Estado = EstadoIncidente.Resuelto;
        FechaResolucion = DateTime.UtcNow;
        Observaciones = observaciones;
    }
}