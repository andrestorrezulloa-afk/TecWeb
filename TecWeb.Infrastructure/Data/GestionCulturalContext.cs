using Microsoft.EntityFrameworkCore;
using TecWeb.Core.Entities;
using System.Reflection;

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
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
