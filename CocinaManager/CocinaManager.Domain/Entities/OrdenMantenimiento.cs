using CocinaManager.Domain.Enums;

namespace CocinaManager.Domain.Entities;

public class OrdenMantenimiento
{
    public Guid Id { get; private set; }
    public Guid HerramientaId { get; private set; }
    public Herramienta Herramienta { get; private set; }
    public string Descripcion { get; private set; }
    public EstadoOrden EstadoOrden { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaResolucion { get; private set; }
    public string? Observaciones { get; private set; }

    private OrdenMantenimiento() { }

    public OrdenMantenimiento(Guid herramientaId, string descripcion)
    {
        Id = Guid.NewGuid();
        HerramientaId = herramientaId;
        Descripcion = descripcion;
        EstadoOrden = EstadoOrden.Pendiente;
        FechaCreacion = DateTime.UtcNow;
    }

    public void Resolver(string observaciones)
    {
        EstadoOrden = EstadoOrden.Resuelta;
        FechaResolucion = DateTime.UtcNow;
        Observaciones = observaciones;
    }

    public void Cancelar(string observaciones)
    {
        EstadoOrden = EstadoOrden.Cancelada;
        Observaciones = observaciones;
    }
}