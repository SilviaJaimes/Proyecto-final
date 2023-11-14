using System.ComponentModel.DataAnnotations;

namespace API.Dtos;
public class AddRoleDto
{
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Contraseña { get; set; }
    [Required]
    public string Role { get; set; }
}
