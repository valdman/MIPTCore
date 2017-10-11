using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Infrastructure;

namespace NavigationHelper
{
    public class NavigationHelper : INavigationHelper
    {
        private readonly INavigationTableRepository _navigationTableRepository;
        
        public NavigationHelper(INavigationTableRepository navigationTableRepository)
        {
            _navigationTableRepository = navigationTableRepository;
        }
        
        public Task<IEnumerable<NavigationTableEntry>> GetNavigationTable()
        {
            return _navigationTableRepository.GetAll();
        }

        public async Task SaveTable(IEnumerable<NavigationTableEntry> table)
        {
            await _navigationTableRepository.DeleteAllNavigatioTableEntriesAsync();
            
            foreach (var capitalsTableEntry in table)
            {
                await _navigationTableRepository.CreateAsync(capitalsTableEntry);
            }
        }
    }
}