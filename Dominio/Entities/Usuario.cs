namespace Dominio.Entities;

public class Usuario : BaseEntity
{
    public string Nombre { get; set; }
    public string CorreoElectronico { get; set; }
    public string Contraseña { get; set; }

    public ICollection<RolUsuario> RolUsuarios { get; set; } 
    public ICollection<Rol> Roles { get; set; }  = new HashSet<Rol>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
}