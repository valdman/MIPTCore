using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Infrastructure;
using Journalist;

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

        public Task<NavigationTableEntry> GetElementById(int id)
        {
            Require.Positive(id, nameof(id));

            return _navigationTableRepository.GetByIdAsync(id);
        }

        public Task<int> CreateElement(NavigationTableEntry element)
        {
            Require.NotNull(element, nameof(element));
            
            return _navigationTableRepository.CreateAsync(element);
        }

        public Task UpdateElement(NavigationTableEntry element)
        {
            Require.NotNull(element, nameof(element));
            
            return _navigationTableRepository.UpdateAsync(element);
        }

        public Task DeleteElement(int id)
        {
            Require.Positive(id, nameof(id));

            return _navigationTableRepository.DeleteAsync(id);
        }
    }
}