using System;
using System.Collections.Generic;

namespace CocinaManager.Domain.Entities;

public class RecepcionViveres
{
    public Guid Id { get; private set; }
    public DateTime Fecha { get; private set; }
    public string? Proveedor { get; private set; }
    public string? Remito { get; private set; }
    public string? RecibidoPor { get; private set; }
    public string? DniRecibidoPor { get; private set; }
    public string? Observaciones { get; private set; }

    // Navegación
    public List<RecepcionViveresLinea> Lineas { get; private set; } = new();

    private RecepcionViveres() { }

    public RecepcionViveres(DateTime fecha, string? proveedor, string? remito, string? recibidoPor, string? dni, string? observaciones)
    {
        Id = Guid.NewGuid();
        Fecha = fecha;
        Proveedor = proveedor;
        Remito = remito;
        RecibidoPor = recibidoPor;
        DniRecibidoPor = dni;
        Observaciones = observaciones;
    }

    public void AddLinea(Guid productoId, string nombreProducto, decimal cantidad, string? unidad, string? observacion)
    {
        var linea = new RecepcionViveresLinea(productoId, nombreProducto, cantidad, unidad, observacion);
        Lineas.Add(linea);
    }

    public void UpdateHeader(string? proveedor, string? remito, string? recibidoPor, string? dni, string? observaciones)
    {
        Proveedor = proveedor;
        Remito = remito;
        RecibidoPor = recibidoPor;
        DniRecibidoPor = dni;
        Observaciones = observaciones;
    }
}