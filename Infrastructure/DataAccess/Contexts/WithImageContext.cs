using CapitalManagment;
using CapitalsTableHelper;
using DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;
using NewsManagment;

namespace DataAccess.Contexts
{
    public class WithImageContext : DbContext
    {
        public DbSet<Capital> Capitals { get; private set; }
        public DbSet<CapitalsTableEntry> CapitalsTableEntries { get; private set; }
        public DbSet<News> News { get; private set; }
        
        public WithImageContext(DbContextOptions<WithImageContext> connectionOptions) : base(connectionOptions)
        {
        }

        private WithImageContext(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new CapitalMap(modelBuilder.Entity<Capital>());
            new PersonMap(modelBuilder.Entity<Person>());
            new NewsMap(modelBuilder.Entity<News>());
            new CapitalTableEntryMap(modelBuilder.Entity<CapitalsTableEntry>());
        }
    }
}