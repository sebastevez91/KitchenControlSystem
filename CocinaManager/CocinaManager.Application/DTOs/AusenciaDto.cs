using System.ComponentModel.DataAnnotations;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.DTOs;

public class AusenciaDto
{
    public Guid Id { get; set; }
    public Guid PersonalId { get; set; }
    public string NombrePersonal { get; set; }
    public TipoAusencia Tipo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int DiasAusencia { get; set; }
    public string RegistradoPor { get; set; }
    public DateTime FechaRegistro { get; set; }
}

public class CreateAusenciaDto
{
    [Required(ErrorMessage = "El personal es obligatorio.")]
    public Guid PersonalId { get; set; }

    [EnumDataType(typeof(TipoAusencia))]
    public TipoAusencia Tipo { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateTime FechaInicio { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    public DateTime FechaFin { get; set; }
}