using CapitalManagment;
using Common;
using DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class CapitalContext : DbContext
    {
        public DbSet<Capital> Capitals { get; private set; }
        
        public CapitalContext(DbContextOptions<CapitalContext> connectionOptions) : base(connectionOptions)
        {
        }

        public CapitalContext(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new CapitalMap(modelBuilder.Entity<Capital>());
        }
    }
}