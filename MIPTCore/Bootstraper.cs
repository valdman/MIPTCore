using AutoMapper;
using CapitalManagment;
using Common;
using Common.Infrastructure;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIPTCore.Authentification.Handlers;
using MIPTCore.Middlewares;
using MIPTCore.Models;
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
            ConfigureAutoMapper();

            _services
                //Register auth middleware
                .AddSingleton<IAuthorizationHandler, IsAuthentificatedAuthHandler>()
                .AddSingleton<IAuthorizationHandler, IsInRoleRoleAuthHandler>()
                //Register other middlewares
                .AddScoped<ErrorHandlingMiddleware>()
                //Register settings
                .Configure<BackendSettings>(_configuration.GetSection("BackendSettings"))

                //RegisterDomain
                .AddScoped<IUserManager, UserManager>()
                .AddScoped<ICapitalManager, CapitalManager>();
        }

        private void ConfigureAutoMapper()
        {
            //AutoMapper
            _services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<User, UserModel>();
                cfg.CreateMap<UserRegistrationModel, User>()
                    .ForMember(t => t.Password, o => o.ResolveUsing(p => new Password(p.Password)));
                cfg.CreateMap<UserUpdateModel, User>();
                cfg.CreateMap<AlumniProfile, AlumniProfileModel>();
                cfg.CreateMap<AlumniProfileModel, AlumniProfile>();

                cfg.CreateMap<CapitalUpdatingModel, Capital>();
                cfg.CreateMap<CapitalCreatingModel, Capital>();
                cfg.CreateMap<Capital, CapitalModel>();
            });
            
        }

        private void ConfigureDatebase()
        {
            _services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<UserContext>(options => options
                    .UseNpgsql(_configuration.GetConnectionString("Postgres")))
                .AddDbContext<CapitalContext>(options => options
                    .UseNpgsql(_configuration.GetConnectionString("Postgres"))
                    .EnableSensitiveDataLogging())

                .AddScoped<IGenericRepository<User>, UserRepository>()
                .AddScoped<IGenericRepository<Capital>, CapitalRepository>();

        }
    }
}