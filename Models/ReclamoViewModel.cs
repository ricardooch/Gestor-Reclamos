using System.ComponentModel.DataAnnotations;

public class ReclamoViewModel
{
    // Datos del Reclamo
    [Required(ErrorMessage = "Debe especificar el proveedor")]
    public string NombreProveedor { get; set; }

    [Required(ErrorMessage = "Debe especificar la dirección del proveedor")]
    public string DireccionProveedor { get; set; }

    [RegularExpression(@"^[267]\d{7}$", ErrorMessage = "El teléfono debe tener 8 dígitos y comenzar con 2, 6 o 7.")]
    public string? TelefonoProveedor { get; set; }

    public decimal? MontoReclamo { get; set; }

    [Required(ErrorMessage = "Debe especificar el detalle del reclamo")]
    public string DetalleReclamo { get; set; }

    // Datos del Consumidor
    [Required(ErrorMessage = "Debe especificar su nombre")]
    public string NombreConsumidor { get; set; }

    [Required(ErrorMessage = "Debe especificar su apellido")]
    public string ApellidoConsumidor { get; set; }

    [Required(ErrorMessage = "El DUI es obligatorio.")]
    [RegularExpression(@"^\d{8}-\d$", ErrorMessage = "El formato del DUI debe ser 00000000-0.")]
    public string DuiConsumidor { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com)$", ErrorMessage = "El correo debe ser válido")]
    public string CorreoElectronico { get; set; }
}
