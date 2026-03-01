namespace CocinaManager.Domain.Entities;

public class Mensaje
{
    public Guid Id { get; private set; }
    public string Remitente { get; private set; }
    public string Destinatario { get; private set; }
    public string Asunto { get; private set; }
    public string Cuerpo { get; private set; }
    public bool Leido { get; private set; }
    public DateTime FechaEnvio { get; private set; }
    public Guid? MensajePadreId { get; private set; }
    public Mensaje? MensajePadre { get; private set; }
    public bool EliminadoPorRemitente { get; private set; }
    public bool EliminadoPorDestinatario { get; private set; }

    private Mensaje() { }

    public Mensaje(string remitente, string destinatario, string asunto, string cuerpo, Guid? mensajePadreId = null)
    {
        Id = Guid.NewGuid();
        Remitente = remitente;
        Destinatario = destinatario;
        Asunto = asunto;
        Cuerpo = cuerpo;
        Leido = false;
        FechaEnvio = DateTime.UtcNow;
        MensajePadreId = mensajePadreId;
        EliminadoPorRemitente = false;
        EliminadoPorDestinatario = false;
    }

    public void MarcarLeido() => Leido = true;

    public void EliminarPorRemitente() => EliminadoPorRemitente = true;

    public void EliminarPorDestinatario() => EliminadoPorDestinatario = true;
}