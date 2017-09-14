using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagment;

namespace DataAccess.Mappings
{
    public class AlumniProfileMap
    {
        public AlumniProfileMap(EntityTypeBuilder<AlumniProfile> e)
        {
            e.HasKey(t => t.Id);
        }
    }
}