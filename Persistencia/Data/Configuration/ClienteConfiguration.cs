using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {

        builder.ToTable("cliente");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
        .HasColumnName("Codigo")
        .IsRequired();
        
        builder.Property(p => p.NombreCliente)
        .HasColumnName("nombreCliente")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.NombreContacto)
        .HasColumnName("nombreContacto")
        .HasColumnType("varchar")
        .HasMaxLength(30)
        .IsRequired();

        builder.Property(p => p.ApellidoContacto)
        .HasColumnName("apellidoContacto")
        .HasColumnType("varchar")
        .HasMaxLength(30)
        .IsRequired();

        builder.Property(p => p.Telefono)
        .HasColumnName("telefono")
        .HasColumnType("varchar")
        .HasMaxLength(15)
        .IsRequired();

        builder.Property(p => p.Fax)
        .HasColumnName("fax")
        .HasColumnType("varchar")
        .HasMaxLength(15)
        .IsRequired();

        builder.Property(p => p.LineaDireccion1)
        .HasColumnName("lineaDireccion1")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.LineaDireccion2)
        .HasColumnName("lineaDireccion2")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.Ciudad)
        .HasColumnName("ciudad")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.Region)
        .HasColumnName("region")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.Pais)
        .HasColumnName("pais")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.CodigoPostal)
        .HasColumnName("codigoPostal")
        .HasColumnType("varchar")
        .HasMaxLength(10)
        .IsRequired();

        builder.HasOne(d => d.Empleado)
        .WithMany(d => d.Clientes)
        .HasForeignKey(d => d.CodigoEmpleado);

        builder.Property(p => p.LimiteCredito)
        .HasColumnName("limiteCredito")
        .HasColumnType("decimal(15,2)")
        .HasMaxLength(100)
        .IsRequired();
    }
}