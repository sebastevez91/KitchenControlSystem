using System.ComponentModel;

namespace CocinaManager.Domain.Enums;

public enum TipoAusencia
{
    Enfermedad = 1,
    Licencia = 2,
    [Description("Ausencia Injustificada")]
    AusenciaInjustificada = 3,
    Vacaciones = 4
}