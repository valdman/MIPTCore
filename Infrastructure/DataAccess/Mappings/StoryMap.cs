using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoriesManagment;

namespace DataAccess.Mappings
{
    public class StoryMap
    {
        public StoryMap(EntityTypeBuilder<Story> mapping)
        {
            mapping.HasQueryFilter(t => !t.IsDeleted);
            
            mapping.HasKey(t => t.Id);
            mapping.Property(t => t.Content);
            
            mapping.HasOne(c => c.Owner).WithMany().IsRequired(true);
        }
    }
}