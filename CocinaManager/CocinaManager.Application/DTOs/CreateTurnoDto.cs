using System.ComponentModel.DataAnnotations;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.DTOs;

public class CreateTurnoDto
{
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; }

    [EnumDataType(typeof(TipoTurno), ErrorMessage = "Tipo de turno inválido.")]
    public TipoTurno TipoTurno { get; set; }

    [Required(ErrorMessage = "El personal es obligatorio.")]
    public Guid PersonalId { get; set; }
}