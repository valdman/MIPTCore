using DonationManagment;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class DonationContext : DbContext
    {
        public DbSet<Donation> Donations { get; private set; }
        
        public DonationContext(DbContextOptions<DonationContext> connectionOptions) : base(connectionOptions)
        {
        }
        
        private DonationContext(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Donation>();

            e.HasQueryFilter(ticket => !ticket.IsDeleted && !ticket.IsDeleted);
            
            e.HasKey(t => t.Id);
        }
    }
}