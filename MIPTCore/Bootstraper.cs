using System;
using Common.Infrastructure;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIPTCore.Authentification.Handlers;
using MIPTCore.Middlewares;
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
                .AddSingleton<IAuthorizationHandler, IsInRoleRoleAuthHandler>()
                //Register other middlewares
                .AddScoped<ErrorHandlingMiddleware>()
                //Register settings
                .Configure<BackendSettings>(_configuration.GetSection("BackendSettings"))
                
                //RegisterDomain
                .AddScoped<IUserManager, UserManager>();
        }
        
        private void ConfigureDatebase()
        {
            _services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<UserContext>(options => options
                    .UseNpgsql(_configuration.GetConnectionString("Postgres")))
            
            .AddScoped<IGenericRepository<User>, UserRepository>();

        }
    }
}