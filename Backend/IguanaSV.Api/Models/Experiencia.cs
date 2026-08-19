namespace IguanaSV.Api.Models;

public class Experiencia
{
    public int Id { get; set; }
    public int PublicacionId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int? DuracionHoras { get; set; }
    public decimal? PrecioAdicional { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}