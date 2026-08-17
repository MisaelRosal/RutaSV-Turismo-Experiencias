public class Publicaciones
{
    public int Id {get; set;}
    public int Anfitrion_id {get; set;}
    public int Categoria_id {get; set;}
    public string Titulo {get; set;} = string.Empty;
    public string Descripcion {get; set;} = string.Empty;
    public int precio_por_noche {get; set;}
    public int capacidad_maxima {get; set;}
    public int camas {get; set;}
    public int banos {get; set;}
    public string Direccion_exacta {get; set;} = string.Empty;
    public int latitud {get; set;}
    public int longitud {get; set;}
    public bool Estado {get; set;} = true;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}