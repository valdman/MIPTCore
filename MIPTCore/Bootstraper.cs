using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddEntityFramework()
                .AddSingleton<DbContextOptions>(new DbContextOptions(_configuration["ConnectionString"]))
                .AddSingleton(typeof(DbSessionProvider<>));
        }

        private void ConfigureDatebase()
        {
            var connection = _configuration["DbConnectionString"];
            var optionsBuilder = DbContextOptions
        }
    }
}