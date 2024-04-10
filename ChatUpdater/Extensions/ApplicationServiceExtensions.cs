using Microsoft.EntityFrameworkCore;
using ChatUpdater.ApplicationCore.Services.Interfaces;
using ChatUpdater.ApplicationCore.Services.Services;
using ChatUpdater.Infrastructure.Data;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Infrastructure.Repository.Repositories;

namespace ChatUpdater.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IDemoService, DemoService>();
            return services;
        }
    }
}
