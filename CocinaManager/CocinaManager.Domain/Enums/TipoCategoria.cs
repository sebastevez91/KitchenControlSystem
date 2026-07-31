using System.ComponentModel;

namespace CocinaManager.Domain.Enums;

public enum TipoCategoria
{
    Verduras = 1,
    Carnes = 2,
    Lácteos = 3,
    Frutas = 4,
    Cereales = 5,
    Legumbres = 6,
    Bebidas = 7,
    Otros = 8,
    [Description("Sin Categoría")]
    Sin_Categoria = 9,
    Condimentos = 10,
}
