namespace IguanaSV.Api.Models;

public class Notificacion
{
    public int Id { get; set; }
    public int ReservaId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public bool? Leida { get; set; }
    public string DestinatarioEmail { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}