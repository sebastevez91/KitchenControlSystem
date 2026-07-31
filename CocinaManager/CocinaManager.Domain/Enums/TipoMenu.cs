using System.ComponentModel;

namespace CocinaManager.Domain.Enums;

public enum TipoMenu
{
    [Description("Desayuno")]
    Desayuno = 1,
    [Description("Almuerzo")]
    Almuerzo = 2,
    [Description("Cena")]
    Cena = 3,
    [Description("Merienda")]
    Merienda = 4,
    [Description("Snack")]
    Snack = 5,
    [Description("Postre")]
    Postre = 6
}
