using CocinaManager.Domain.Enums;

namespace CocinaManager.Domain.Entities;

public class Ausencia
{
    public Guid Id { get; private set; }
    public Guid PersonalId { get; private set; }
    public Personal Personal { get; private set; }
    public TipoAusencia Tipo { get; private set; }
    public DateTime FechaInicio { get; private set; }
    public DateTime FechaFin { get; private set; }
    public string RegistradoPor { get; private set; }
    public DateTime FechaRegistro { get; private set; }

    public int DiasAusencia => (FechaFin - FechaInicio).Days + 1;

    private Ausencia() { }

    public Ausencia(Guid personalId, TipoAusencia tipo, DateTime fechaInicio, DateTime fechaFin, string registradoPor)
    {
        Id = Guid.NewGuid();
        PersonalId = personalId;
        Tipo = tipo;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        RegistradoPor = registradoPor;
        FechaRegistro = DateTime.UtcNow;
    }
}