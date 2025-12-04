using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TecWeb.Core.Entities;
using TecWeb.Infrastructure.Data.Configurations;

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
        public virtual DbSet<UserSecurity> UserSecurities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserSecurityConfiguration());

            OnModelCreatingPartial(modelBuilder);  // Dejamos esta línea
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
