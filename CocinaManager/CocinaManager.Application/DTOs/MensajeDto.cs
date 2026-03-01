using System.ComponentModel.DataAnnotations;

namespace CocinaManager.Application.DTOs;

public class MensajeDto
{
    public Guid Id { get; set; }
    public string Remitente { get; set; }
    public string Destinatario { get; set; }
    public string Asunto { get; set; }
    public string Cuerpo { get; set; }
    public bool Leido { get; set; }
    public DateTime FechaEnvio { get; set; }
    public Guid? MensajePadreId { get; set; }
}

public class CreateMensajeDto
{
    [Required(ErrorMessage = "El destinatario es obligatorio.")]
    public string Destinatario { get; set; }

    [Required(ErrorMessage = "El asunto es obligatorio.")]
    [MaxLength(200)]
    public string Asunto { get; set; }

    [Required(ErrorMessage = "El cuerpo es obligatorio.")]
    [MaxLength(2000)]
    public string Cuerpo { get; set; }

    public Guid? MensajePadreId { get; set; }
}