using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapitalsTableHelper
{
    public interface ICapitalsTableHelper
    {
        Task<IEnumerable<CapitalsTableEntry>> GetTableForCapitals();
        Task<CapitalsTableEntry> GetEntryForCapital(int capitalId);

        Task UpdateEntryForCapital(CapitalsTableEntry capitalsTableEntry);
        Task SaveTable(IEnumerable<CapitalsTableEntry> table);
    }
}