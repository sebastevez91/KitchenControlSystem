using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.DTOs;

public class MovimientoStockDto
{
    public Guid Id { get; set; }
    public Guid ProductoId { get; set; }
    public string NombreProducto { get; set; }
    public TipoMovimiento TipoMovimiento { get; set; }
    public decimal Cantidad { get; set; }
    public DateTime Fecha { get; set; }
    public string Observacion { get; set; }
}