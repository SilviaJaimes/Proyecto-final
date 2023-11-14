namespace API.Dtos;

public class RolUsuarioDto
{
    public int IdRolFk { get; set; }
    public RolDto Rol { get; set; }
    public int IdUsuarioFk { get; set; }
    public UsuarioDto Usuario { get; set; }
}