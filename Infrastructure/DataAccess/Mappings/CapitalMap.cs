using CapitalManagment;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mappings
{
    public class CapitalMap
    {
        public CapitalMap(EntityTypeBuilder<Capital> e)
        {
            e.HasQueryFilter(t => !t.IsDeleted);
            
            e.HasKey(t => t.Id);
            e.HasIndex(t => t.Name).IsUnique();
            e.Property(t => t.Description).IsRequired();

            e.Property(t => t.Need).IsRequired();
            e.Property(t => t.Given).IsRequired();
            e.Property(t => t.FullPageUri);

            e.HasOne(c => c.Image).WithMany().IsRequired(false);

            e.HasMany(c => c.Founders).WithOne().HasForeignKey("FoundedCapitalId").IsRequired(false);
            e.HasMany(c => c.Recivers).WithOne().HasForeignKey("ReciverOfCapitalId").IsRequired(false);
        }
    }
}