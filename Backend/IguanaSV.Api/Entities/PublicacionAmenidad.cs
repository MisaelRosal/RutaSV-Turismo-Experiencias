using System;
using System.Collections.Generic;

namespace IguanaSV.Api.Entities;

public partial class PublicacionAmenidad
{
    public int PublicacionId { get; set; }

    public int AmenidadId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Amenidade? Amenidad { get; set; }

    public virtual Publicacione? Publicacion { get; set; }
}
