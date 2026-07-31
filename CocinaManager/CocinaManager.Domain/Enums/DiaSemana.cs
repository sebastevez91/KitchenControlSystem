using System.ComponentModel;

namespace CocinaManager.Domain.Enums;

public enum DiaSemana
{
    [Description("Lunes")]
    Lunes = 1,
    [Description("Martes")]
    Martes = 2,
    [Description("Miércoles")]
    Miércoles = 3,
    [Description("Jueves")]
    Jueves = 4,
    [Description("Viernes")]
    Viernes = 5,
    [Description("Sábado")]
    Sábado = 6,
    [Description("Domingo")]
    Domingo = 7
}
