using System.ComponentModel.DataAnnotations;

namespace CocinaManager.Application.DTOs;

public class CreatePersonalDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El documento es obligatorio.")]
    [MaxLength(20, ErrorMessage = "El documento no puede superar 20 caracteres.")]
    public string Documento { get; set; }

    [Required(ErrorMessage = "El cargo es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El cargo no puede superar 100 caracteres.")]
    public string Cargo { get; set; }
}