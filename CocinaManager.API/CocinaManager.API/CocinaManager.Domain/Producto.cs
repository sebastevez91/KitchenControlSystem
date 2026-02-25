namespace CocinaManager.API.CocinaManager.Domain;

public class Producto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string UnidadMedida { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
}