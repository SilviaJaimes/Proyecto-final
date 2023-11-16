using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
{
    public void Configure(EntityTypeBuilder<Empleado> builder)
    {

        builder.ToTable("empleado");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
        .HasColumnName("codigo")
        .HasColumnType("int")
        .HasMaxLength(11)
        .IsRequired();

        builder.Property(p => p.Nombre)
        .HasColumnName("nombre")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.Apellido1)
        .HasColumnName("apellido1")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.Apellido2)
        .HasColumnName("apellido2")
        .HasColumnType("varchar")
        .HasMaxLength(50);

        builder.Property(p => p.Extension)
        .HasColumnName("extension")
        .HasColumnType("varchar")
        .HasMaxLength(10)
        .IsRequired();

        builder.Property(p => p.Email)
        .HasColumnName("email")
        .HasColumnType("varchar")
        .HasMaxLength(100)
        .IsRequired();

        builder.HasOne(d => d.Oficina)
        .WithMany(d => d.Empleados)
        .HasForeignKey(d => d.CodigoOficina)
        .IsRequired();

        builder.HasOne(p => p.Jefe)        
        .WithMany(p => p.Empleados)         
        .HasForeignKey(p => p.CodigoJefe)
        .IsRequired(false);

        builder.Property(p => p.Puesto)
        .HasColumnName("puesto")
        .HasColumnType("varchar")
        .HasMaxLength(50);
    }
}