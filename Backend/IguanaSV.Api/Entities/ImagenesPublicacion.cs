using System;
using System.Collections.Generic;

namespace IguanaSV.Api.Entities;

public partial class ImagenesPublicacion
{
    public int Id { get; set; }

    public int PublicacionId { get; set; }

    public string Url { get; set; } = null!;

    public bool? EsPrincipal { get; set; }

    public int? Orden { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Publicacione? Publicacion { get; set; }
}
