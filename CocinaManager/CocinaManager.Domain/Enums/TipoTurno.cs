using System.ComponentModel;

namespace CocinaManager.Domain.Enums;

public enum TipoTurno
{
    Manana = 1,
    Tarde = 2,
    Noche = 3,
    [Description("Turno Completo")]
    Turno_Completo = 4
}
