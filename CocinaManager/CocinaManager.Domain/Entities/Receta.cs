namespace CocinaManager.Domain.Entities;

public class Receta
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string Descripcion { get; private set; }
    public int Porciones { get; private set; }
    public bool Activo { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    public ICollection<RecetaIngrediente> Ingredientes { get; private set; } = new List<RecetaIngrediente>();

    private Receta() { }

    public Receta(string nombre, string descripcion, int porciones)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        Descripcion = descripcion;
        Porciones = porciones;
        Activo = true;
        FechaCreacion = DateTime.UtcNow;
    }

    public void Actualizar(string nombre, string descripcion, int porciones)
    {
        Nombre = nombre;
        Descripcion = descripcion;
        Porciones = porciones;
    }

    public void Desactivar() => Activo = false;
}