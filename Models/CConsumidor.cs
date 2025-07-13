using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTemp_Ochoa.Models;

public partial class CConsumidor
{
    public int IdConsumidor { get; set; }

    public string NombreConsumidor { get; set; } = null!;

    public string ApellidoConsumidor { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string DuiConsumidor { get; set; } = null!;

    public bool? Activo { get; set; }

    public virtual ICollection<TReclamo> TReclamos { get; set; } = new List<TReclamo>();
}
