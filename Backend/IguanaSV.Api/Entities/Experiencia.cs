using System;
using System.Collections.Generic;

namespace IguanaSV.Api.Entities;

public partial class Experiencia
{
    public int Id { get; set; }

    public int PublicacionId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int? DuracionHoras { get; set; }

    public decimal? PrecioAdicional { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Publicacione? Publicacion { get; set; }
}
