using System;
using System.Collections.Generic;

namespace PTemp_Ochoa.Models;

public partial class TReclamo
{
    public int IdReclamo { get; set; }

    public string NombreProveedor { get; set; } = null!;

    public string DireccionProveedor { get; set; } = null!;

    public string DetalleReclamo { get; set; } = null!;

    public string? TelefonoProveedor { get; set; }

    public decimal? MontoReclamo { get; set; }

    public DateTime FechaIngreso { get; set; }

    public DateTime? FechaRevision { get; set; }

    public int? IdEmpleado { get; set; }

    public int? IdConsumidor { get; set; }

    public int? IdEstado { get; set; }

    public bool Activo { get; set; }

    public virtual CConsumidor? IdConsumidorNavigation { get; set; }

    public virtual CEmpleado? IdEmpleadoNavigation { get; set; }

    public virtual CEstado? IdEstadoNavigation { get; set; }

    public virtual TAsesorium? TAsesoria { get; set; }
    public virtual TAviso? TAviso { get; set; }
}
