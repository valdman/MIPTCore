using CapitalManagment;
using CapitalsTableHelper;
using Common;
using DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class CapitalContext : DbContext
    {
        public DbSet<Capital> Capitals { get; private set; }
        public DbSet<CapitalsTableEntry> CapitalsTableEntries { get; private set; }
        
        public CapitalContext(DbContextOptions<CapitalContext> connectionOptions) : base(connectionOptions)
        {
        }

        private CapitalContext(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new CapitalMap(modelBuilder.Entity<Capital>());
            new PersonMap(modelBuilder.Entity<Person>());
            
            var mapping = modelBuilder.Entity<CapitalsTableEntry>();

            mapping.HasKey(t => t.CapitalId);
            mapping.HasOne<Capital>().WithOne().HasForeignKey<CapitalsTableEntry>(t => t.CapitalId).IsRequired();
        }
    }
}