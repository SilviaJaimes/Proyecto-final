namespace API.Dtos;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string CorreoElectronico { get; set; }
    public string Contraseña { get; set; }
}