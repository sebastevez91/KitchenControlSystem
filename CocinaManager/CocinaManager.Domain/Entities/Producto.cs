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
    public DateTime FechaCreacion { get; private set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; private set; }

    // Ahora la FK es obligatoria
    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; }

    private Producto() { }

    // Constructor exige categoriaId
    public Producto(string nombre, string unidadMedida, decimal stockMinimo, Guid categoriaId)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        UnidadMedida = unidadMedida;
        StockMinimo = stockMinimo;
        StockActual = 0;
        Activo = true;
        CategoriaId = categoriaId;
    }

    public void AumentarStock(decimal cantidad)
    {
        StockActual += cantidad;
    }

    public void DisminuirStock(decimal cantidad)
    {
        StockActual -= cantidad;
    }

    // Método para asignar o cambiar categoría desde la entidad
    public void AsignarCategoria(Guid categoriaId)
    {
        CategoriaId = categoriaId;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Editar(string nombre, string unidadMedida, decimal stockMinimo, Guid categoriaId)
    {
        Nombre = nombre;
        UnidadMedida = unidadMedida;
        StockMinimo = stockMinimo;
        CategoriaId = categoriaId;
        FechaModificacion = DateTime.UtcNow;
    }
}
