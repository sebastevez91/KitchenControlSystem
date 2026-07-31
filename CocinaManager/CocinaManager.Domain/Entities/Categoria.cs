using CocinaManager.Domain.Enums;
using System;
using System.Collections.Generic;

namespace CocinaManager.Domain.Entities;

public class Categoria
{
    public Guid Id { get; private set; }
    public TipoCategoria TipoCategoria { get; private set; }
    public string Nombre { get; private set; }
    public string Descripcion { get; private set; } = string.Empty;

    // Colección de productos para la relación inversa (EF Core)
    private readonly List<Producto> _productos = new();
    public IReadOnlyCollection<Producto> Productos => _productos.AsReadOnly();

    private Categoria() { }

    // Se crea la categoría a partir del enum; Nombre se deriva del enum por defecto
    public Categoria(TipoCategoria tipoCategoria, string descripcion = "")
    {
        Id = Guid.NewGuid();
        TipoCategoria = tipoCategoria;
        Nombre = tipoCategoria.ToString();
        Descripcion = descripcion ?? string.Empty;
    }
}
