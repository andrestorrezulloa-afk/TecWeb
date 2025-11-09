using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWeb.Core.Entities;

namespace TecWeb.Infrastructure.Data.Configurations
{
    public class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            // 🔑 Clave primaria
            builder.HasKey(e => e.EventoId)
                   .HasName("PK_Evento");

            // 🧱 Nombre de la tabla
            builder.ToTable("Eventos");

            // 🏷️ Título
            builder.Property(e => e.Titulo)
                   .HasMaxLength(200)
                   .IsUnicode(false);

            // 📝 Descripción
            builder.Property(e => e.Descripcion)
                   .HasMaxLength(500)
                   .IsUnicode(false);

            // 📍 Lugar
            builder.Property(e => e.Lugar)
                   .HasMaxLength(200)
                   .IsUnicode(false);

            // 📅 Fecha del evento
            builder.Property(e => e.Fecha)
                   .HasColumnType("datetime");

            // 🕒 Hora de inicio
            builder.Property(e => e.HoraInicio)
                   .HasColumnType("datetime");

            // 🕕 Hora de fin
            builder.Property(e => e.HoraFin)
                   .HasColumnType("datetime");

            // 👥 Aforo máximo
            builder.Property(e => e.AforoMaximo)
                   .IsRequired();

            // 🔗 Relación con Usuario (muchos eventos pertenecen a un usuario)
            builder.HasOne(e => e.Usuario)
                   .WithMany(u => u.Eventos)
                   .HasForeignKey(e => e.UsuarioId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Evento_Usuario");

            // 🔗 Relación con Inscripciones (un evento tiene muchas inscripciones)
            builder.HasMany(e => e.Inscripciones)
                   .WithOne(i => i.Evento)
                   .HasForeignKey(i => i.EventoId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
