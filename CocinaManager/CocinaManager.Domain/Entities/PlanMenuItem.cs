using CocinaManager.Domain.Enums;
using System;

namespace CocinaManager.Domain.Entities;

public class PlanMenuItem
{
    public Guid Id { get; private set; }
    public Guid PlanMenuId { get; private set; }
    public Guid RecetaId { get; private set; }
    public Receta? Receta { get; private set; }

    public RolPlato? Rol {  get; private set; }

    private PlanMenuItem() { }

    public PlanMenuItem(Guid planMenuId, Guid recetaId, RolPlato? rol = null)
    {
        Id = Guid.NewGuid();
        PlanMenuId = planMenuId;
        RecetaId = recetaId;
        Rol = rol;
    }
}