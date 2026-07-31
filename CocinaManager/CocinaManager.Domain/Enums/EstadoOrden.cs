using System.ComponentModel;

namespace CocinaManager.Domain.Enums;

public enum EstadoOrden
{
    Pendiente = 1,
    [Description("En Proceso")]
    EnProceso = 2,
    Resuelta = 3,
    Cancelada = 4
}