using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagment;

namespace DataAccess.Mappings
{
    public class AlumniProfileMap
    {
        public AlumniProfileMap(EntityTypeBuilder<AlumniProfile> e)
        {
            e.HasKey(_ => _.Id);
            e.HasOne(_ => _.User)
                .WithMany().IsRequired()
                .HasForeignKey(p => p.UserFk);
        }
    }
}