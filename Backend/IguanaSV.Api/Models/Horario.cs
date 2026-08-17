namespace IguanaSV.Api.Models;

public class Horario
{
    public int Id { get; set; }
    public int PublicacionId { get; set; }
    public int DiaSemana { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }
    public bool? Disponible { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}