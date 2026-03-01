using System.ComponentModel.DataAnnotations;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.DTOs;

public class HerramientaDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public EstadoHerramienta Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
}

public class CreateHerramientaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
    public string Nombre { get; set; }

    [MaxLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
    public string Descripcion { get; set; }
}