using System;
using System.Collections.Generic;

namespace PTemp_Ochoa.Models;

public partial class TAsesorium
{
    public int IdAsesoria { get; set; }

    public DateTime FechaIngreso { get; set; }

    public string MotivoAsesoria { get; set; } = null!;

    public string RespuestaAsesoria { get; set; } = null!;

    public int? IdReclamo { get; set; }

    public bool Activo { get; set; }

    public virtual TReclamo? IdReclamoNavigation { get; set; }
}
