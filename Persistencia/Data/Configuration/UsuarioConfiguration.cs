using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;
public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuario");

        builder.HasKey(x => x.Id);

        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.Nombre)
            .HasColumnName("nombre")
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.CorreoElectronico)
            .HasColumnName("correoElectronico")
            .HasColumnType("varchar")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.Contraseña)
            .HasColumnName("contraseña")
            .HasColumnType("varchar")
            .HasMaxLength(255)
            .IsRequired();

        builder
            .HasMany(p => p.Roles)
            .WithMany(r => r.Usuarios)
            .UsingEntity<RolUsuario>(

                j => j
                .HasOne(pt => pt.Rol)
                .WithMany(t => t.RolUsuarios)
                .HasForeignKey(ut => ut.IdRolFk),

                j => j
                .HasOne(et => et.Usuario)
                .WithMany(et => et.RolUsuarios)
                .HasForeignKey(el => el.IdUsuarioFk),

                j =>
                {
                    j.ToTable("rolUsuario");
                    j.HasKey(t => new { t.IdUsuarioFk, t.IdRolFk });
                });

        builder
            .HasMany(p => p.RefreshTokens)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.UsuarioId);
    }
}
