public class Municipio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Departamento_id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}