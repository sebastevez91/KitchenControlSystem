using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.DTOs;

public class PersonalDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Documento { get; set; }
    public string Cargo { get; set; }
    public EstadoPersonal Estado { get; set; }
}