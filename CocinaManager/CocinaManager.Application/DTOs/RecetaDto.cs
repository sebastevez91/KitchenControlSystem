using System.ComponentModel.DataAnnotations;

namespace CocinaManager.Application.DTOs;

public class RecetaDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public List<RecetaIngredienteDto> Ingredientes { get; set; } = new();
}

public class RecetaIngredienteDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public decimal Cantidad { get; set; }
    public string UnidadMedida { get; set; }
}

public class CreateRecetaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(150)]
    public string Nombre { get; set; }

    [MaxLength(1000)]
    public string Descripcion { get; set; }

    public List<CreateRecetaIngredienteDto> Ingredientes { get; set; } = new();
}

public class CreateRecetaIngredienteDto
{
    [Required]
    public string Nombre { get; set; }
    public decimal Cantidad { get; set; }
    public string UnidadMedida { get; set; }
}

public class UpdateRecetaDto
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(150)]
    public string Nombre { get; set; }

    [MaxLength(1000)]
    public string Descripcion { get; set; }

    public List<CreateRecetaIngredienteDto> Ingredientes { get; set; } = new();
}