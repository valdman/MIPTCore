using CapitalManagment;
using CapitalsTableHelper;
using DataAccess.Mappings;
using DonationManagment;
using Microsoft.EntityFrameworkCore;
using NewsManagment;
using StoriesManagment;

namespace DataAccess.Contexts
{
    public class WithImageContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Capital> Capitals { get; private set; }
        public DbSet<CapitalsTableEntry> CapitalsTableEntries { get; private set; }
        public DbSet<News> News { get; private set; }
		public DbSet<Story> Stories { get; private set; }
        public DbSet<Donation> Donations { get; private set; }
        
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
            new StoryMap(modelBuilder.Entity<Story>());
            new CapitalTableEntryMap(modelBuilder.Entity<CapitalsTableEntry>());
        }
    }
}