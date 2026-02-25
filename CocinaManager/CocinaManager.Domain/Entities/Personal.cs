using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CocinaManager.Domain.Enums;

namespace CocinaManager.Domain.Entities;

public class Personal
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string Documento { get; private set; }
    public string Cargo { get; private set; }
    public EstadoPersonal Estado { get; private set; }
    public DateTime FechaIngreso { get; private set; }
    public bool Activo { get; private set; }

    private Personal() { }

    public Personal(string nombre, string documento, string cargo)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        Documento = documento;
        Cargo = cargo;
        Estado = EstadoPersonal.Activo;
        FechaIngreso = DateTime.UtcNow;
        Activo = true;
    }

    public void CambiarEstado(EstadoPersonal nuevoEstado)
    {
        Estado = nuevoEstado;
    }
}
