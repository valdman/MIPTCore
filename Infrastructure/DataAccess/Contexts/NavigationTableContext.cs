using Microsoft.EntityFrameworkCore;
using NavigationHelper;

namespace DataAccess.Contexts
{
    public class NavigationTableContext : DbContext
    {
        public DbSet<NavigationTableEntry> NavigationTable { get; private set; }
        
        public NavigationTableContext(DbContextOptions<NavigationTableContext> connectionOptions) : base(connectionOptions)
        {
        }

        private NavigationTableContext(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<NavigationTableEntry>();

            e.HasKey(c => c.Id);
        }
    }
}