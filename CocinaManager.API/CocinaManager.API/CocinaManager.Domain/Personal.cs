namespace CocinaManager.API.CocinaManager.Domain;

public class Personal
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Documento { get; set; }
    public string Cargo { get; set; }
    public EstadoPersonal Estado { get; set; }
}

public enum EstadoPersonal
{
    Activo,
    Enfermo,
    Licencia,
    Ausente
}
