using System.Collections.Generic;
using System.Threading.Tasks;

namespace NavigationHelper
{
    public interface INavigationHelper
    {
        Task<IEnumerable<NavigationTableEntry>> GetNavigationTable();
        Task<NavigationTableEntry> GetElementById(int id);

        Task<int> CreateElement(NavigationTableEntry element);
        Task UpdateElement(NavigationTableEntry element);
        
        Task DeleteElement(int id);
    }
}