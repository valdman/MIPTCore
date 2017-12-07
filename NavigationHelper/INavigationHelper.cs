using System.Collections.Generic;
using System.Threading.Tasks;

namespace NavigationHelper
{
    public interface INavigationHelper
    {
        IEnumerable<NavigationTableEntry> GetNavigationTable();
        NavigationTableEntry GetElementById(int id);

        int CreateElement(NavigationTableEntry element);
        void UpdateElement(NavigationTableEntry element);
        
        void DeleteElement(int id);
    }
}