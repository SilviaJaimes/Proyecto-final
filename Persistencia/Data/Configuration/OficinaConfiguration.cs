using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class OficinaConfiguration : IEntityTypeConfiguration<Oficina>
{
    public void Configure(EntityTypeBuilder<Oficina> builder)
    {

        builder.ToTable("oficina");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
        .HasColumnName("codigo")
        .IsRequired();

        builder.Property(p => p.Ciudad)
        .HasColumnName("ciudad")
        .HasColumnType("varchar")
        .HasMaxLength(30)
        .IsRequired();

        builder.Property(p => p.Pais)
        .HasColumnName("pais")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.Region)
        .HasColumnName("region")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.CodigoPostal)
        .HasColumnName("codigoPostal")
        .HasColumnType("varchar")
        .HasMaxLength(10)
        .IsRequired();

        builder.Property(p => p.Telefono)
        .HasColumnName("telefono")
        .HasColumnType("varchar")
        .HasMaxLength(20)
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
    }
}