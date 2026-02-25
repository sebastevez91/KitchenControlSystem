using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CocinaManager.Domain.Enums;

namespace CocinaManager.Domain.Entities;

public class Turno
{
    public Guid Id { get; private set; }
    public DateTime Fecha { get; private set; }
    public TipoTurno TipoTurno { get; private set; }

    public Guid PersonalId { get; private set; }
    public Personal Personal { get; private set; }

    private Turno() { }

    public Turno(DateTime fecha, TipoTurno tipoTurno, Guid personalId)
    {
        Id = Guid.NewGuid();
        Fecha = fecha;
        TipoTurno = tipoTurno;
        PersonalId = personalId;
    }
}
