namespace CocinaManager.Domain.Entities;

public class RegistroComensales
{
    public Guid Id { get; private set; }
    public DateTime Fecha { get; private set; }
    public int Cantidad { get; private set; }
    public DateTime FechaRegistro { get; private set; }

    private RegistroComensales() { }

    public RegistroComensales(DateTime fecha, int cantidad)
    {
        Id = Guid.NewGuid();
        Fecha = fecha.Date;
        Cantidad = cantidad;
        FechaRegistro = DateTime.Now;
    }

    public void ActualizarCantidad(int cantidad)
    {
        Cantidad = cantidad;
        FechaRegistro = DateTime.Now;
    }
}
