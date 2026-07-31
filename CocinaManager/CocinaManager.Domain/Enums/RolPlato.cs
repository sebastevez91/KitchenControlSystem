using System.ComponentModel;

namespace CocinaManager.Domain.Enums;

public enum RolPlato
{
    [Description("Entrada")]
    Entrada = 1,
    [Description("Plato principal")]
    PlatoPrincipal = 2
}