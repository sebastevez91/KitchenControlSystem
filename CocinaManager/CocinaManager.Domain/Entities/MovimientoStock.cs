using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CocinaManager.Domain.Enums;

namespace CocinaManager.Domain.Entities;

public class MovimientoStock
{
    public Guid Id { get; private set; }
    public Guid ProductoId { get; private set; }
    public Producto Producto { get; private set; }
    public TipoMovimiento TipoMovimiento { get; private set; }
    public decimal Cantidad { get; private set; }
    public DateTime Fecha { get; private set; }
    public string Observacion { get; private set; }

    private MovimientoStock() { }

    public MovimientoStock(Guid productoId, TipoMovimiento tipoMovimiento, decimal cantidad, string observacion, DateTime fecha)
    {
        Id = Guid.NewGuid();
        ProductoId = productoId;
        TipoMovimiento = tipoMovimiento;
        Cantidad = cantidad;
        Fecha = fecha;
        Observacion = observacion;
    }
}
