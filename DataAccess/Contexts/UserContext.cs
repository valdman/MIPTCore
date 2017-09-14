using DataAccess.EFLogging;
using DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagment;

namespace DataAccess.Contexts
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; private set; }
        public DbSet<AlumniProfile> AlumniProfiles { get; set; }
        
        public UserContext(DbContextOptions<UserContext> connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserMap(modelBuilder.Entity<User>());
            new AlumniProfileMap(modelBuilder.Entity<AlumniProfile>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var lf = new LoggerFactory();
            lf.AddProvider(new EfToNpgsqlLoggerProvider());
            optionsBuilder.UseLoggerFactory(lf);
        }
    }
}