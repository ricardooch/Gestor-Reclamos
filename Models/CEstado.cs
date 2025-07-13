using System;
using System.Collections.Generic;

namespace PTemp_Ochoa.Models;

public partial class CEstado
{
    public int IdEstado { get; set; }

    public string? NombreEstado { get; set; }

    public virtual ICollection<TReclamo> TReclamos { get; set; } = new List<TReclamo>();
}
