using CocinaManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocinaManager.Application.Interfaces;

public interface IRegistroComensalesRepository
{
    Task<RegistroComensales?> GetByFechaAsync(DateTime fecha);
    Task<List<RegistroComensales>> GetByFechasAsync(IEnumerable<DateTime> fechas);
    Task AddAsync(RegistroComensales registro);
    Task SaveChangesAsync();
}
