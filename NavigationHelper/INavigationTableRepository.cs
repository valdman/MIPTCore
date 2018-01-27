using Common.Infrastructure;

namespace NavigationHelper
{
    public interface INavigationTableRepository : IGenericRepository<NavigationTableEntry>
    {
        void DeleteAllNavigatioTableEntries();
    }
}