using BannerHelper;
using Microsoft.EntityFrameworkCore;
using NavigationHelper;

namespace DataAccess.Contexts
{
    public class BannerContext : DbContext
    {
        public DbSet<BannerElement> Banner { get; private set; }
        
        public BannerContext(DbContextOptions<BannerContext> connectionOptions) : base(connectionOptions)
        {
        }

        private BannerContext(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<NavigationTableEntry>();

            e.HasKey(c => c.Id);
        }
    }
}