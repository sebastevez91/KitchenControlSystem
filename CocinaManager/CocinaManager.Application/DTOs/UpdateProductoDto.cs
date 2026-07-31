using System.ComponentModel.DataAnnotations;

namespace CocinaManager.Application.DTOs;

public class UpdateProductoDto
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
    [MaxLength(50, ErrorMessage = "La unidad de medida no puede superar 50 caracteres.")]
    public string UnidadMedida { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor a 0.")]
    public decimal StockMinimo { get; set; }

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public Guid CategoriaId { get; set; }
}