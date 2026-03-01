namespace CocinaManager.Domain.Entities;

public class RecetaIngrediente
{
    public Guid Id { get; private set; }
    public Guid RecetaId { get; private set; }
    public Receta Receta { get; private set; }
    public string Nombre { get; private set; }
    public decimal Cantidad { get; private set; }
    public string UnidadMedida { get; private set; }

    private RecetaIngrediente() { }

    public RecetaIngrediente(Guid recetaId, string nombre, decimal cantidad, string unidadMedida)
    {
        Id = Guid.NewGuid();
        RecetaId = recetaId;
        Nombre = nombre;
        Cantidad = cantidad;
        UnidadMedida = unidadMedida;
    }
}