using System.ComponentModel.DataAnnotations;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.DTOs;

public class IncidenteDto
{
    public Guid Id { get; set; }
    public string Descripcion { get; set; }
    public TipoIncidente Tipo { get; set; }
    public EstadoIncidente Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaResolucion { get; set; }
    public string RegistradoPor { get; set; }
    public string? Observaciones { get; set; }
}

public class CreateIncidenteDto
{
    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [MaxLength(1000)]
    public string Descripcion { get; set; }

    [EnumDataType(typeof(TipoIncidente))]
    public TipoIncidente Tipo { get; set; }
}

public class ResolverIncidenteDto
{
    [MaxLength(500)]
    public string? Observaciones { get; set; }
}