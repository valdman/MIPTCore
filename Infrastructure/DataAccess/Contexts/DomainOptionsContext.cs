using Common;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class DomainOptionsContext : DbContext
    {
        public DbSet<DomainOptions> DomainOptionsDB { get; set; }
        
        public DomainOptionsContext(DbContextOptions<DomainOptionsContext> connectionOptions) : base(connectionOptions)
        {
        }
        
        private DomainOptionsContext(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<DomainOptions>();

            e.HasKey(c => c.Id);
        }
    }
}