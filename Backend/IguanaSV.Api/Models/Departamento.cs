namespace IguanaSV.Api.Models;

public class Departamento
{
    public int Id{get; set;}
    public string Nombre{get; set;} = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}