namespace CocinaManager.Application.DTOs;

public class ViveresResultadoDto
{
    public DateTime Fecha { get; set; }
    public int Comensales { get; set; }
    public List<string> Recetas { get; set; } = new();
    public List<ViveresItemDto> Items { get; set; } = new();
}

public class ViveresItemDto
{
    public string Nombre { get; set; } = string.Empty;
    public string UnidadMedida { get; set; } = string.Empty;
    public decimal CantidadNecesaria { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public bool Disponible { get; set; }
    public Guid? ProductoId { get; set; }
}
