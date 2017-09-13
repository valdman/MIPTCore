using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagment;

namespace DataAccess.Mappings
{
    public class AlumniProfileMap
    {
        public AlumniProfileMap(EntityTypeBuilder<AlumniProfile> e)
        {
            e.HasKey(t => t.Id);
            
            e.HasOne<User>().WithOne(t => t.AlumniProfile)
                .HasForeignKey<User>("AlumniProfileFK_Shadow").IsRequired(false);
        }
    }
}