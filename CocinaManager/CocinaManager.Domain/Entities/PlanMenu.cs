namespace CocinaManager.Domain.Entities;

public class PlanMenu
{
    public Guid Id { get; private set; }
    public DateTime Fecha { get; private set; }
    public Guid RecetaId { get; private set; }
    public Receta Receta { get; private set; }
    public string TipoComida { get; private set; } // Desayuno, Almuerzo, Merienda, Cena
    public string? Observaciones { get; private set; }

    private PlanMenu() { }

    public PlanMenu(DateTime fecha, Guid recetaId, string tipoComida, string? observaciones)
    {
        Id = Guid.NewGuid();
        Fecha = fecha;
        RecetaId = recetaId;
        TipoComida = tipoComida;
        Observaciones = observaciones;
    }
}