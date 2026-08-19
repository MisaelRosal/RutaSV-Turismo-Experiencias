namespace IguanaSV.Api.Models;

public class Amenidades
{
    public int Id{get; set;}
    public string nombre{get; set;} = string.Empty;
    public string icono{get; set;} = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}