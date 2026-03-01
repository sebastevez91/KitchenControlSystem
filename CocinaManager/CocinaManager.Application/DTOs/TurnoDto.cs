using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.DTOs;

public class TurnoDto
{
    public Guid Id { get; set; }
    public DateTime Fecha { get; set; }
    public TipoTurno TipoTurno { get; set; }
    public Guid PersonalId { get; set; }
    public string NombrePersonal { get; set; }
}
