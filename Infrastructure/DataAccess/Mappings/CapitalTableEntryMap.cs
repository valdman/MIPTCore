using CapitalManagment;
using CapitalsTableHelper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mappings
{
    public class CapitalTableEntryMap
    {
        public CapitalTableEntryMap(EntityTypeBuilder<CapitalsTableEntry> mapping)
        {
            mapping.HasKey(t => t.CapitalId);
            mapping.HasOne<Capital>().WithOne().HasForeignKey<CapitalsTableEntry>(t => t.CapitalId).IsRequired();
        }
    }
}