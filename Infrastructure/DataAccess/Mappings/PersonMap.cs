using CapitalManagment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mappings
{
    public class PersonMap
    {
        public PersonMap(EntityTypeBuilder<Person> e)
        {
            e.HasKey(p => p.Id);
        }
    }
}