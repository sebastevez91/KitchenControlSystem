using System.ComponentModel.DataAnnotations;

namespace CocinaManager.Application.DTOs;

public class PlanMenuDto
{
    public Guid Id { get; set; }
    public DateTime Fecha { get; set; }
    public Guid RecetaId { get; set; }
    public string NombreReceta { get; set; }
    public string TipoComida { get; set; }
    public string? Observaciones { get; set; }
}

public class CreatePlanMenuDto
{
    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public Guid RecetaId { get; set; }

    [Required]
    public string TipoComida { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }
}