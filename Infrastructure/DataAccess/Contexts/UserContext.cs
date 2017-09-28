using Common;
using DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;
using UserManagment;

namespace DataAccess.Contexts
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; private set; }
        public DbSet<AlumniProfile> AlumniProfiles { get; private set; }
        public DbSet<Password> UserPasswords { get; set; }
        
        public UserContext(DbContextOptions<UserContext> connectionOptions) : base(connectionOptions)
        {
        }
        
        private UserContext(DbContextOptions connectionOptions) : base(connectionOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserMap(modelBuilder.Entity<User>());
            new AlumniProfileMap(modelBuilder.Entity<AlumniProfile>());
        }
    }
}