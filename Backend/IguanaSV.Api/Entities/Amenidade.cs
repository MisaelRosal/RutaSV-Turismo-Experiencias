using System;
using System.Collections.Generic;

namespace IguanaSV.Api.Entities;

public partial class Amenidade
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Icono { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PublicacionAmenidad> PublicacionAmenidads { get; set; } = new List<PublicacionAmenidad>();
}
