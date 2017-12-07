using System.Threading.Tasks;
using Common.Infrastructure;

namespace NavigationHelper
{
    public interface INavigationTableRepository : IGenericRepository<NavigationTableEntry>
    {
        void DeleteAllNavigatioTableEntries();
    }
}