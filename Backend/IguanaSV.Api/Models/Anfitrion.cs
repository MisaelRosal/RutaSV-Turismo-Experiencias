namespace IguanaSV.Api.Models;

public class Anfitrion
{
    public int Id{get; set;}
    public int Municipio_id{get; set;}
    public string Nombre{get; set;} = string.Empty;
    public string Email{get; set;} = string.Empty;
    public string Telefono{get; set;} = string.Empty;
    public string PhotoUrl{get; set;} = string.Empty;
    public bool Verificado{get; set;} = false;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}