using System;

namespace CocinaManager.Domain.Entities;

public class RecepcionViveresLinea
{
    public Guid Id { get; private set; }
    public Guid RecepcionViveresId { get; private set; }
    public Guid ProductoId { get; private set; }
    public string NombreProducto { get; private set; }
    public decimal Cantidad { get; private set; }
    public string? Unidad { get; private set; }
    public string? Observacion { get; private set; }

    private RecepcionViveresLinea() { }

    public RecepcionViveresLinea(Guid productoId, string nombreProducto, decimal cantidad, string? unidad, string? observacion)
    {
        Id = Guid.NewGuid();
        ProductoId = productoId;
        NombreProducto = nombreProducto;
        Cantidad = cantidad;
        Unidad = unidad;
        Observacion = observacion;
    }
}