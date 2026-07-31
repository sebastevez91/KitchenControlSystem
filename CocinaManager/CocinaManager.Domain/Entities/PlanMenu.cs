using System;
using System.Linq;
using System.Collections.Generic;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Domain.Entities;

public class PlanMenu
{
    public Guid Id { get; private set; }
    public DiaSemana DiaSemana { get; private set; }
    public TipoMenu TipoMenu { get; private set; }
    public DateTime FechaCreacion { get; private set; } = DateTime.Now;
    public DateTime? FechaActualizacion { get; private set; }
    public ICollection<PlanMenuItem> Items { get; private set; } = new List<PlanMenuItem>();

    private PlanMenu() { }

    public PlanMenu(DiaSemana diaSemana, TipoMenu tipoMenu)
    {
        Id = Guid.NewGuid();
        DiaSemana = diaSemana;
        TipoMenu = tipoMenu;
        FechaCreacion = DateTime.Now;
    }

    public void AgregarReceta(Guid recetaId)
    {
        if (!Items.Any(i => i.RecetaId == recetaId))
        {
            Items.Add(new PlanMenuItem(Id, recetaId));
            FechaActualizacion = DateTime.Now;
        }
    }

    public void RemoverItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            Items.Remove(item);
            FechaActualizacion = DateTime.Now;
        }
    }

    public void ReemplazarReceta(Guid recetaId)
    {
        Items.Clear();
        Items.Add(new PlanMenuItem(Id, recetaId));
        FechaActualizacion = DateTime.Now;
    }
}