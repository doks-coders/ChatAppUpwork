using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatUpdater.Models.Entities;

namespace ChatUpdater.Infrastructure.Configurations
{
    /// <summary>
    /// Entity configuration for Connection
    /// </summary>
    internal class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder.HasOne(u => u.Group)
                .WithMany(u => u.Connections)
                .HasForeignKey(u => u.GroupName)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
