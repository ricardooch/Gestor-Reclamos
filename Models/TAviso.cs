using System;
using System.Collections.Generic;

namespace PTemp_Ochoa.Models;

public partial class TAviso
{
    public int IdAviso { get; set; }

    public DateTime FechaIngreso { get; set; }

    public string DetalleAviso { get; set; } = null!;

    public int? IdReclamo { get; set; }

    public bool Activo { get; set; }

    public virtual TReclamo? IdReclamoNavigation { get; set; }
}
