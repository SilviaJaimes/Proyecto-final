using System.ComponentModel.DataAnnotations;

namespace API.Dtos;
public class LoginDto
{
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Contraseña { get; set; }
}
