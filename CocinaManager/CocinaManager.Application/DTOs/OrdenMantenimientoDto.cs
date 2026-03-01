using System.ComponentModel.DataAnnotations;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.DTOs;

public class OrdenMantenimientoDto
{
    public Guid Id { get; set; }
    public Guid HerramientaId { get; set; }
    public string NombreHerramienta { get; set; }
    public string Descripcion { get; set; }
    public EstadoOrden EstadoOrden { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaResolucion { get; set; }
    public string? Observaciones { get; set; }
}

public class CreateOrdenMantenimientoDto
{
    [Required(ErrorMessage = "La herramienta es obligatoria.")]
    public Guid HerramientaId { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [MaxLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
    public string Descripcion { get; set; }
}

public class ResolverOrdenDto
{
    [MaxLength(500)]
    public string? Observaciones { get; set; }
}