using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocinaManager.Application.Interfaces;

public interface IComensalesService
{
    Task<int?> GetCantidadAsync(DateTime fecha);
    Task RegistrarAsync(DateTime fecha, int cantidad);
}
