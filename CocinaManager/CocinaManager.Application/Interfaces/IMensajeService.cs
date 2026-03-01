using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IMensajeService
{
    Task<List<MensajeDto>> GetRecibidosAsync(string usuario);
    Task<List<MensajeDto>> GetEnviadosAsync(string usuario);
    Task<MensajeDto?> GetByIdAsync(Guid id, string usuario);
    Task<int> GetNoLeidosCountAsync(string usuario);
    Task<MensajeDto> EnviarAsync(CreateMensajeDto dto, string remitente);
    Task<MensajeDto> ResponderAsync(Guid mensajePadreId, string cuerpo, string remitente);
    Task<bool> EliminarAsync(Guid id, string usuario);
}