using System.Threading.Tasks;
using Common.Infrastructure;

namespace CapitalsTableHelper
{
    public interface ICapitalsTableEntryRepository : IGenericRepository<CapitalsTableEntry>
    {
        void DeleteAllCapitalTableEntries();
    }
}