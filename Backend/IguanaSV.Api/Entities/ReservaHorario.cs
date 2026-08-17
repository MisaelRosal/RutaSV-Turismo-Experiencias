using System;
using System.Collections.Generic;

namespace IguanaSV.Api.Entities;

public partial class ReservaHorario
{
    public int ReservaId { get; set; }

    public int HorarioId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Horario Horario { get; set; } = null!;

    public virtual Reserva Reserva { get; set; } = null!;
}
