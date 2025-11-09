using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWeb.Core.Entities;

namespace TecWeb.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
           
            builder.HasKey(e => e.UsuarioId)
                   .HasName("PK_Usuario");
            builder.ToTable("Usuarios");

            builder.Property(e => e.Correo)
                   .HasMaxLength(150)
                   .IsUnicode(false);

            builder.Property(e => e.Nombre)
                   .HasMaxLength(100)
                   .IsUnicode(false);

           
            builder.Property(e => e.Apellido)
                   .HasMaxLength(100)
                   .IsUnicode(false);

            
            builder.Property(e => e.Telefono)
                   .HasMaxLength(20)
                   .IsUnicode(false);

            builder.Property(e => e.Rol)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            builder.Property(e => e.FechaRegistro)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("(getdate())");

            builder.HasMany(e => e.Eventos)
                   .WithOne(d => d.Usuario)
                   .HasForeignKey(d => d.UsuarioId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(e => e.Inscripciones)
                   .WithOne(d => d.Usuario)
                   .HasForeignKey(d => d.UsuarioId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
    
}
