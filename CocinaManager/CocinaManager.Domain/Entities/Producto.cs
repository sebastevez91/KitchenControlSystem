using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocinaManager.Domain.Entities;

public class Producto
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string UnidadMedida { get; private set; }
    public decimal StockActual { get; private set; }
    public decimal StockMinimo { get; private set; }
    public bool Activo { get; private set; }

    private Producto() { }

    public Producto(string nombre, string unidadMedida, decimal stockMinimo)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        UnidadMedida = unidadMedida;
        StockMinimo = stockMinimo;
        StockActual = 0;
        Activo = true;
    }

    public void AumentarStock(decimal cantidad)
    {
        StockActual += cantidad;
    }

    public void DisminuirStock(decimal cantidad)
    {
        StockActual -= cantidad;
    }
}
