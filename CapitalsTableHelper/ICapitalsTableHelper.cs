using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapitalsTableHelper
{
    public interface ICapitalsTableHelper
    {
        IEnumerable<CapitalsTableEntry> GetTableForCapitals();
        void SaveTable(IEnumerable<CapitalsTableEntry> table);
    }
}