using System.ComponentModel.DataAnnotations;

namespace API.Dtos;
public class RegisterDto
{
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Contraseña { get; set; }
    [Required]
    public string CorreoElectronico { get; set; }
}
