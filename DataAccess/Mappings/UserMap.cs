using System.ComponentModel.DataAnnotations.Schema;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            e.OwnsOne(t => t.Password, p =>
            {
                p.Property(_ => _.Hash);
            });
            e.Property(t => t.IsMiptAlumni).IsRequired();
            e.OwnsOne(t => t.AlumniProfile, p =>
            {
                p.Property(_ => _.YearOfGraduation);
                p.Property(_ => _.Diploma);
                p.Property(_ => _.Faculty);
            });
            e.Property(t => t.Role);
            e.Property(t => t.AuthentificatedAt);
        }
    }
}