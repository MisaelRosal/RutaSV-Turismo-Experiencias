namespace IguanaSV.Api.Models;

public class ImagenesPublicacion
{
    public int Id { get; set; }
    public int PublicacionId { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool? EsPrincipal { get; set; }
    public int? Orden { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}