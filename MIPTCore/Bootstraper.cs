using System;
using System.Diagnostics;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;

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
        }

        private void ConfigureDatebase()
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql(_configuration.GetConnectionString("Postgres"))
                .Options;
            var sessionProvider = new DbSessionProvider(options);
            
            _services
                .AddEntityFrameworkNpgsql()
                .AddScoped(typeof(DbSessionProvider), _ => new DbSessionProvider(options))
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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