using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapitalsTableHelper
{
    public interface ICapitalsTableHelper
    {
        Task<IEnumerable<CapitalsTableEntry>> GetTableForCapitals();
        Task SaveTable(IEnumerable<CapitalsTableEntry> table);
    }
}