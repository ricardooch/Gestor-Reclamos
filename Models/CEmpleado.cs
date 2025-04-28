using System;
using System.Collections.Generic;

namespace PTemp_Ochoa.Models;

public partial class CEmpleado
{
    public int IdEmpleado { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Usuario { get; set; }

    public string? Clave { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<TReclamo> TReclamos { get; set; } = new List<TReclamo>();
}
