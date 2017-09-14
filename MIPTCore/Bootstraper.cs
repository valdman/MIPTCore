using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIPTCore.Authentification.Handlers;
using UserManagment;

namespace MIPTCore
{
    public class Bootstraper
    {
        private readonly IServiceCollection _services;
        private readonly IConfigurationRoot _configuration;

        public Bootstraper(IServiceCollection services, IConfigurationRoot configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public void Configure()
        {
            ConfigureDatebase();
            
            _services
                //Register auth middleware
                .AddSingleton<IAuthorizationHandler, IsAuthentificatedAuthHandler>()
                .AddSingleton<IAuthorizationHandler, IsInRoleRoleAuthHandler>();
        }
        
        private void ConfigureDatebase()
        {
            _services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<UserContext>(options => options
                    .UseNpgsql(_configuration.GetConnectionString("Postgres"))
                    .EnableSensitiveDataLogging())
                .AddScoped<GenericRepository<User>, UserRepository>()
                .AddScoped<UserRepository>();

        }
    }
}