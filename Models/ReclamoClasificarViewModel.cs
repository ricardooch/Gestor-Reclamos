using PTemp_Ochoa.Models;
using System.ComponentModel.DataAnnotations;

public class ReclamoClasificarViewModel
{
    public int IdReclamo { get; set; }
    public string NombreProveedor { get; set; }
    public string DireccionProveedor { get; set; }
    public string DetalleReclamo { get; set; }
    public string? TelefonoProveedor { get; set; }
    public decimal? MontoReclamo { get; set; }
    public string NombreConsumidor { get; set; }
    public string ApellidoConsumidor { get; set; }
    public string CorreoElectronico { get; set; }
    public string DuiConsumidor { get; set; }
    
    public int IdEstado { get; set; }
    public string? MotivoAsesoria { get; set; }
    public string? RespuestaAsesoria { get; set; }
    public string? DetalleAviso { get; set; }
    public List<CEstado> Estados { get; set; } = new List<CEstado>();
}
