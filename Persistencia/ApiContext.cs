using System.Reflection;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistencia;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {}
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<RolUsuario> RolUsuarios { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}