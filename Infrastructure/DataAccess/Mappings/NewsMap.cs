using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsManagment;

namespace DataAccess.Mappings
{
    public class NewsMap
    {
        public NewsMap(EntityTypeBuilder<News> mapping)
        {
            mapping.HasQueryFilter(t => !t.IsDeleted);

            mapping.HasKey(t => t.Id);
            mapping.Property(t => t.Name);
            mapping.Property(t => t.Description);
            mapping.Property(t => t.Content);

            mapping.Property(t => t.Date).IsRequired(false);
            mapping.HasOne(c => c.Image).WithMany().IsRequired(false);
            mapping.HasOne(c => c.PreviewImage).WithMany().IsRequired(false);
        }
    }
}