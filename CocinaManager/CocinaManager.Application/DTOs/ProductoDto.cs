namespace CocinaManager.Application.DTOs;

public class ProductoDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string UnidadMedida { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public bool StockBajo => StockActual <= StockMinimo;
    // Ahora obligatorio en DTO también
    public Guid CategoriaId { get; set; } 
}
