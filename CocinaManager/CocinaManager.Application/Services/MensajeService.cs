using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CocinaManager.Application.Services;

public class MensajeService : IMensajeService
{
    private readonly IMensajeRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ILogger<MensajeService> _logger;

    public MensajeService(IMensajeRepository repo, IUsuarioRepository usuarioRepo, ILogger<MensajeService> logger)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
        _logger = logger;
    }

    public async Task<List<MensajeDto>> GetRecibidosAsync(string usuario)
        => (await _repo.GetRecibidosAsync(usuario)).Select(Map).ToList();

    public async Task<List<MensajeDto>> GetEnviadosAsync(string usuario)
        => (await _repo.GetEnviadosAsync(usuario)).Select(Map).ToList();

    public async Task<MensajeDto?> GetByIdAsync(Guid id, string usuario)
    {
        var mensaje = await _repo.GetByIdAsync(id);
        if (mensaje == null) return null;

        // Marcar como leído si es el destinatario
        if (mensaje.Destinatario == usuario && !mensaje.Leido)
        {
            mensaje.MarcarLeido();
            await _repo.SaveChangesAsync();
        }
        return Map(mensaje);
    }

    public async Task<int> GetNoLeidosCountAsync(string usuario)
        => await _repo.GetNoLeidosCountAsync(usuario);

    public async Task<MensajeDto> EnviarAsync(CreateMensajeDto dto, string remitente)
    {
        _logger.LogInformation("Enviando mensaje de {Remitente} a {Destinatario}", remitente, dto.Destinatario);
        var mensaje = new Mensaje(remitente, dto.Destinatario, dto.Asunto, dto.Cuerpo, dto.MensajePadreId);
        await _repo.AddAsync(mensaje);
        await _repo.SaveChangesAsync();
        return Map(mensaje);
    }

    public async Task<MensajeDto> ResponderAsync(Guid mensajePadreId, string cuerpo, string remitente)
    {
        var padre = await _repo.GetByIdAsync(mensajePadreId)
            ?? throw new KeyNotFoundException("Mensaje no encontrado.");

        var respuesta = new Mensaje(
            remitente,
            padre.Remitente == remitente ? padre.Destinatario : padre.Remitente,
            $"Re: {padre.Asunto}",
            cuerpo,
            mensajePadreId);

        await _repo.AddAsync(respuesta);
        await _repo.SaveChangesAsync();
        return Map(respuesta);
    }

    public async Task<bool> EliminarAsync(Guid id, string usuario)
    {
        var mensaje = await _repo.GetByIdAsync(id);
        if (mensaje == null) return false;

        if (mensaje.Destinatario == usuario) mensaje.EliminarPorDestinatario();
        else if (mensaje.Remitente == usuario) mensaje.EliminarPorRemitente();

        await _repo.SaveChangesAsync();
        return true;
    }

    private static MensajeDto Map(Mensaje m) => new()
    {
        Id = m.Id,
        Remitente = m.Remitente,
        Destinatario = m.Destinatario,
        Asunto = m.Asunto,
        Cuerpo = m.Cuerpo,
        Leido = m.Leido,
        FechaEnvio = m.FechaEnvio,
        MensajePadreId = m.MensajePadreId
    };
}