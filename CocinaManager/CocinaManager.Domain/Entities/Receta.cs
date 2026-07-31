namespace CocinaManager.Domain.Entities;

public class Receta
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string Descripcion { get; private set; }
    public bool Activo { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    public ICollection<RecetaIngrediente> Ingredientes { get; private set; } = new List<RecetaIngrediente>();

    private Receta() { }

    public Receta(string nombre, string descripcion)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        Descripcion = descripcion;
        Activo = true;
        FechaCreacion = DateTime.UtcNow;
    }

    public void Actualizar(string nombre, string descripcion)
    {
        Nombre = nombre;
        Descripcion = descripcion;
    }

    public void Desactivar() => Activo = false;

    public void Activar() => Activo = true;
}