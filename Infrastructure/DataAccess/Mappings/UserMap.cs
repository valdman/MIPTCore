using Common.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagment;

namespace DataAccess.Mappings
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<User> e)
        {
            e.HasQueryFilter(t => !t.IsDeleted);
            
            e.HasKey(t => t.Id);
            e.Property(t => t.FirstName).IsRequired();
            e.Property(t => t.LastName).IsRequired();
            e.HasIndex(t => t.Email);
            e.Property(t => t.IsMiptAlumni).IsRequired();

            e.HasOne(t => t.Password).WithOne()
                .IsRequired()
                .HasForeignKey<User>("PasswordId");

            e.HasOne(t => t.AlumniProfile).WithMany().IsRequired(false);

        }
    }
    
    public class AlumniProfileMap
    {
        public AlumniProfileMap(EntityTypeBuilder<AlumniProfile> e)
        {
            e.HasKey(t => t.Id);
        }
    }

    public class PasswordMap
    {
        public PasswordMap(EntityTypeBuilder<Password> e)
        {
            e.HasKey(p => p.Id);
        }
    }
}