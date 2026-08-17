using System;
using System.Collections.Generic;

namespace IguanaSV.Api.Entities;

public partial class Municipio
{
    public int Id { get; set; }

    public int DepartamentoId { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Anfitrione> Anfitriones { get; set; } = new List<Anfitrione>();

    public virtual Departamento Departamento { get; set; } = null!;
}
