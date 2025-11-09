using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWeb.Core.Entities;

namespace TecWeb.Infrastructure.Data.Configurations
{
    public class InscripcioneConfiguration : IEntityTypeConfiguration<Inscripcione>
    {
        public void Configure(EntityTypeBuilder<Inscripcione> builder)
        {
           
            builder.HasKey(e => e.InscripcionId)
                   .HasName("PK_Inscripcion");

           
            builder.ToTable("Inscripciones");

            
            builder.Property(e => e.FechaInscripcion)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("(getdate())");

           
            builder.Property(e => e.Asistencia)
                   .HasDefaultValue(false);

            
            builder.HasOne(e => e.Usuario)
                   .WithMany(u => u.Inscripciones)
                   .HasForeignKey(e => e.UsuarioId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Inscripcion_Usuario");

            
            builder.HasOne(e => e.Evento)
                   .WithMany(ev => ev.Inscripciones)
                   .HasForeignKey(e => e.EventoId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Inscripcion_Evento");

            builder.HasIndex(e => new { e.UsuarioId, e.EventoId })
                   .IsUnique()
                   .HasDatabaseName("UX_Usuario_Evento");
        }
    }
}
