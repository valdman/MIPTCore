using System;
using System.Diagnostics;
using Common;
using Common.Infrastructure;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIPTCore.Authentification.Handlers;
using MIPTCore.Models.ModelValidators;
using Npgsql.EntityFrameworkCore.PostgreSQL;
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
            var contextOptions = new DbContextOptionsBuilder()
                .UseNpgsql(_configuration.GetConnectionString("Postgres"))
                .Options;
            var sessionProvider = new DbSessionProvider(contextOptions);
            
            _services
                .AddEntityFrameworkNpgsql()
                .AddSingleton(typeof(DbSessionProvider), _ => new DbSessionProvider(contextOptions))
                .AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            sessionProvider.Database.EnsureCreated();
            
            //ForceCreateSchema();

            void ForceCreateSchema()
            {
                var databaseCreator =
                (RelationalDatabaseCreator)sessionProvider.Database.GetService<IDatabaseCreator>();
                if (!databaseCreator.EnsureCreated())
                {
                    databaseCreator.CreateTables();
                }
                
            }
        }
    }
}