using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChatUpdater.Models.Entities;
using System.Reflection;

namespace ChatUpdater.Infrastructure.Data
{
    /// <summary>
    /// DbContext for our Application
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, AppRole, Guid,
        IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<AppUserRole> UserRoles { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GCContacts> GroupContacts { get; set; }


        public DbSet<GroupChat> GroupChats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}