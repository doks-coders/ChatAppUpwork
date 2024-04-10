
using ChatUpdater.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatUpdater.Infrastructure.Configurations
{
    public class GroupContactConfiguration : IEntityTypeConfiguration<GCContacts>
    {
        public void Configure(EntityTypeBuilder<GCContacts> builder)
        {

            builder.HasOne(u => u.AppUser)
                .WithMany(u => u.GroupContacts)
                .HasForeignKey(u => u.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.AppGroupChat)
                .WithMany(u => u.GroupContacts)
                .HasForeignKey(u => u.AppGroupId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
