using System.Collections.Generic;
using System.Threading.Tasks;

namespace NavigationHelper
{
    public interface INavigationHelper
    {
        Task<IEnumerable<NavigationTableEntry>> GetNavigationTable();
        Task SaveTable(IEnumerable<NavigationTableEntry> table);
    }
}