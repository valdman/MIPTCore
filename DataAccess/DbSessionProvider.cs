using System;
using System.Diagnostics;
using Common;
using DataAccess.EFLogging;
using DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagment;

namespace DataAccess
{
    public class DbSessionProvider : DbContext
    {
        public DbSessionProvider(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new AlumniProfileMap(modelBuilder.Entity<AlumniProfile>());
            new UserMap(modelBuilder.Entity<User>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var lf = new LoggerFactory();
            lf.AddProvider(new EfToNpgsqlLoggerProvider());
            optionsBuilder.UseLoggerFactory(lf);
        }
    }
}