using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TecWeb.Models;

public partial class GestionCulturalContext : DbContext
{
    public GestionCulturalContext()
    {
    }

    public GestionCulturalContext(DbContextOptions<GestionCulturalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Inscripcione> Inscripciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.EventoId).HasName("PK__Eventos__1EEB592179C004AD");

            entity.Property(e => e.Lugar).HasMaxLength(200);
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Eventos__Usuario__3B75D760");
        });

        modelBuilder.Entity<Inscripcione>(entity =>
        {
            entity.HasKey(e => e.InscripcionId).HasName("PK__Inscripc__168316B917774E42");

            entity.HasIndex(e => new { e.UsuarioId, e.EventoId }, "UQ__Inscripc__2AD3522BD006D60F").IsUnique();

            entity.Property(e => e.Asistencia).HasDefaultValue(false);
            entity.Property(e => e.FechaInscripcion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Evento).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.EventoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inscripci__Event__4222D4EF");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inscripci__Usuar__412EB0B6");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B875386799");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A19F5190FC9").IsUnique();

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
