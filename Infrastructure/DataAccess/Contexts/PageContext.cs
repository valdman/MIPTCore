using Microsoft.EntityFrameworkCore;
using PagesManagment;

namespace DataAccess.Contexts
{
    public class PageContext : DbContext
    {
        public DbSet<Page> Pages { get; private set; }
        
        public PageContext(DbContextOptions<PageContext> connectionOptions) : base(connectionOptions)
        {
        }

        private PageContext(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Page>();

            e.HasQueryFilter(ticket => !ticket.IsDeleted);
            e.HasKey(c => c.Id);
            e.HasIndex(c => c.Name).IsUnique();
            e.HasIndex(c => c.Url).IsUnique();
            e.Property(c => c.Data);
        }
    }
}