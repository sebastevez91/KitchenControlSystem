#nullable enable
using System;
using System.Collections.Generic;

namespace CocinaManager.Application.DTOs;

public class RecepcionViveresLineaDto
{
    public Guid Id { get; set; }
    public Guid MovimientoStockId { get; set; } // <-- propiedad necesaria para el mapeo
    public Guid ProductoId { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public string? Unidad { get; set; }
    public string? Observacion { get; set; }
}

public class CreateRecepcionViveresDto
{
    public DateTime Fecha { get; set; }
    public string? Proveedor { get; set; }
    public string? Remito { get; set; }
    public string? RecibidoPor { get; set; }
    public string? DniRecibidoPor { get; set; }
    public string? Observaciones { get; set; }
    public List<RecepcionViveresLineaDto> Lineas { get; set; } = new();
}

public class RecepcionViveresDto
{
    public Guid Id { get; set; }
    public DateTime Fecha { get; set; }
    public string? Proveedor { get; set; }
    public string? Remito { get; set; }
    public string? RecibidoPor { get; set; }
    public string? DniRecibidoPor { get; set; }
    public string? Observaciones { get; set; }
    public List<RecepcionViveresLineaDto> Lineas { get; set; } = new();
}

public class CreateRecepcionPdfDto
{
    public List<Guid> MovimientoIds { get; set; } = new();
    public bool GuardarRegistro { get; set; } = false;
    public string? Observacion { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}