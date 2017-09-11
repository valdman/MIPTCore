using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagment;

namespace DataAccess.Mappings
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<User> e)
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.FirstName).IsRequired();
            e.Property(t => t.LastName).IsRequired();
            e.Property(t => t.Email).IsRequired();
            e.Property(t => t.Password).IsRequired();
            e.Property(t => t.IsMiptAlumni).IsRequired();
            e.Property(t => t.AlumniProfile);
            e.Property(t => t.Role);
            e.Property(t => t.AuthentificatedAt);
        }
    }
}