using CocinaManager.Domain.Enums;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CocinaManager.Application.DTOs;

public class CreatePlanMenuDto
{
    public DiaSemana DiaSemana{ get; set; }
    public TipoMenu TipoMenu { get; set; }
    public List<Guid> RecetaIds { get; set; } = new();
}

public class PlanMenuDto
{
    public PlanMenuDto(Guid id, DiaSemana diaSemana, TipoMenu tipoMenu, List<PlanMenuItemDto> recetas)
    {
        Id = id;
        DiaSemana = diaSemana;
        TipoMenu = tipoMenu;
        Recetas = recetas;
    }
    public Guid Id { get; set; }
    public DiaSemana DiaSemana { get; set; }
    public TipoMenu TipoMenu { get; set; }
    public List<PlanMenuItemDto> Recetas { get; set; }
}

public class PlanMenuItemDto
{
    public PlanMenuItemDto(Guid id, Guid recetaId, string? recetaNombre, RolPlato? rol = null)
    {
        Id = id;
        RecetaId = recetaId;
        RecetaNombre = recetaNombre;
        Rol = rol;
    }
    public Guid Id { get; set; }
    public Guid RecetaId { get; set; }
    public string? RecetaNombre { get; set; }
    public RolPlato? Rol { get; set; }
}

public class ComidaDto
{
    public TipoMenu TipoMenu { get; set; }
    public List<PlanMenuItemDto> Recetas { get; set; } = new();
}

public class DiaMenuDto
{
    public DiaSemana DiaSemana { get; set; }
    public DateTime Fecha { get; set; } // fecha real de ESTA semana para ese día
    public int? CantidadComensales { get; set; }
    public List<ComidaDto> Comidas { get; set; } = new();
}

public class MenuSemanalDto
{
    public List<DiaMenuDto> Dias { get; set; } = new();
}

public class RegistrarComensalesDto
{
    public DateTime Fecha { get; set; }
    public int Cantidad { get; set; }
}

public class SetRecetaCeldaDto
{
    public DiaSemana DiaSemana { get; set; }
    public TipoMenu TipoMenu { get; set; }
    public Guid RecetaId { get; set; }
    public RolPlato? Rol {  get; set; }
}

public class RecetaSimpleDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}