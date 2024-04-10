using ChatUpdater.Infrastructure.Data;
using ChatUpdater.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatUpdater.Extensions
{
    public static class RunMigration
    {
        public static async Task<WebApplication> Migrate(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var db = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                    var migrations = await db.Database.GetPendingMigrationsAsync();
                    if (migrations.Any())
                    {
                        await db.Database.MigrateAsync();
                    }
                    var logger = services.GetService<ILogger<Program>>();
                    logger.LogInformation("Migration Successful");
                }
                catch (Exception ex)
                {
                    var logger = services.GetService<ILogger<Program>>();
                    logger.LogError(ex, "An Error Occurred during Migration");
                }

            }

            return app;
        }
    }
}
