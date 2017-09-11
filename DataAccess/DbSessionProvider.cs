using System;
using System.Diagnostics;
using Common;
using DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;
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
            var userMap = new UserMap(modelBuilder.Entity<User>());
        }
    }
}