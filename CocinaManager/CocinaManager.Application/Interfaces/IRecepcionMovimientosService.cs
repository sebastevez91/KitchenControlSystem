using System;
using System.Threading.Tasks;

namespace CocinaManager.Application.Interfaces;

public interface IRecepcionMovimientosService
{
    /// <summary>
    /// Genera un PDF con los movimientos de tipo Entrada del día indicado
    /// sin modificar la base de datos.
    /// </summary>
    Task<byte[]> GeneratePdfEntradasPorFechaAsync(DateTime fecha, string? proveedor = null, string? remito = null, string? recibidoPor = null, string? dni = null, string? observaciones = null);
}