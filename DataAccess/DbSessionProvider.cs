using System;
using System.Diagnostics;
using Common.Entities;
using DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class DbSessionProvider : DbContext
    {
        public DbSessionProvider(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserMap(modelBuilder.Entity<User>());
        }
    }
}