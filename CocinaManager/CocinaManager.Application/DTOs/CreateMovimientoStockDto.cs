using System.ComponentModel.DataAnnotations;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.DTOs;

public class CreateMovimientoStockDto
{
    [Required(ErrorMessage = "El producto es obligatorio.")]
    public Guid ProductoId { get; set; }

    [EnumDataType(typeof(TipoMovimiento), ErrorMessage = "Tipo de movimiento inválido.")]
    public TipoMovimiento TipoMovimiento { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public decimal Cantidad { get; set; }

    [MaxLength(500, ErrorMessage = "La observación no puede superar 500 caracteres.")]
    public string? Observacion { get; set; }

    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}