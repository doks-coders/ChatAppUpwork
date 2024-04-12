using ChatUpdater.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatUpdater.Infrastructure.Configurations
{
    /// <summary>
    /// Entity configuration for AppRole
    /// </summary>
    public class AppRoleConfigurations : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasMany(k => k.AppUserRoles)
                .WithOne(u => u.AppRole)
                .HasForeignKey(u => u.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
