using Microsoft.EntityFrameworkCore;
using TecWeb.Core.Entities;

namespace TecWeb.Infrastructure.Data
{
    public partial class GestionCulturalContext : DbContext
    {
        public GestionCulturalContext()
        {
        }

        public GestionCulturalContext(DbContextOptions<GestionCulturalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Evento> Eventos { get; set; } = null!;
        public virtual DbSet<Inscripcione> Inscripciones { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasKey(e => e.EventoId);

                entity.Property(e => e.Lugar).HasMaxLength(200);
                entity.Property(e => e.Titulo).HasMaxLength(200);

                entity.HasOne(d => d.Usuario).WithMany(p => p.Eventos)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Inscripcione>(entity =>
            {
                entity.HasKey(e => e.InscripcionId);

                entity.HasIndex(e => new { e.UsuarioId, e.EventoId }).IsUnique();

                entity.Property(e => e.Asistencia).HasDefaultValue(false);
                entity.Property(e => e.FechaInscripcion)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Evento).WithMany(p => p.Inscripciones)
                    .HasForeignKey(d => d.EventoId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Usuario).WithMany(p => p.Inscripciones)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.UsuarioId);
                entity.HasIndex(e => e.Correo).IsUnique();

                entity.Property(e => e.Apellido).HasMaxLength(100);
                entity.Property(e => e.Correo).HasMaxLength(150);
                entity.Property(e => e.FechaRegistro)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Rol).HasMaxLength(50);
                entity.Property(e => e.Telefono).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
