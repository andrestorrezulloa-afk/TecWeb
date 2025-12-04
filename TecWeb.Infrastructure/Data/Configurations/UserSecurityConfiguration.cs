using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; // NUEVO
using TecWeb.Core.Entities;
using TecWeb.Core.Enum;

namespace TecWeb.Infrastructure.Data.Configurations
{
    public class UserSecurityConfiguration : IEntityTypeConfiguration<UserSecurity>
    {
        public void Configure(EntityTypeBuilder<UserSecurity> builder)
        {
            builder.ToTable("UserSecurity");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Login)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            // OPCIÓN 3: Usando EnumToStringConverter (la más simple)
            builder.Property(e => e.UserRole)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasConversion(new EnumToStringConverter<RoleType>());
        }
    }
}