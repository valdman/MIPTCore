using Microsoft.EntityFrameworkCore;
using UserManagment;

namespace DataAccess.Contexts
{
    public class TicketContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; private set; }
        
        public TicketContext(DbContextOptions<TicketContext> connectionOptions) : base(connectionOptions)
        {
        }
        
        private TicketContext(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Ticket>();

            e.HasQueryFilter(ticket => !ticket.IsDeleted && !ticket.IsCompleted);
            
            e.HasKey(t => t.Id);
            e.HasIndex(t => t.Token);
        }
    }
}