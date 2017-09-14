using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagment;

namespace DataAccess.Mappings
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<User> e)
        {
            e.HasQueryFilter(@object => !@object.IsDeleted);
            
            e.HasKey(t => t.Id);
            e.Property(t => t.FirstName).IsRequired();
            e.Property(t => t.LastName).IsRequired();
            e.HasIndex(t => t.Email).IsUnique();
            e.Property(t => t.IsMiptAlumni).IsRequired();
            
            e.OwnsOne(t => t.Password, p =>
            {
                p.Property(_ => _.Hash);
            });

            e.HasOne(t => t.AlumniProfile).WithMany().IsRequired(false);

        }
    }
}