using System.ComponentModel;

namespace CocinaManager.Domain.Enums;

public enum EstadoHerramienta
{
    Operativa = 1,
    [Description("En Reparación")]
    EnReparacion = 2,
    [Description("Fuera de Servicio")]
    FueraDeServicio = 3
}