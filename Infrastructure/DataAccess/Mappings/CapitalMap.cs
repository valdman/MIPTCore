using CapitalManagment;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mappings
{
    public class CapitalMap
    {
        public CapitalMap(EntityTypeBuilder<Capital> e)
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Name).IsRequired();
            e.Property(t => t.Description).IsRequired();

            e.Property(t => t.Need).IsRequired();
            e.Property(t => t.Given).IsRequired();

            e.HasOne(c => c.Image).WithMany().IsRequired(false);
        }
    }
}