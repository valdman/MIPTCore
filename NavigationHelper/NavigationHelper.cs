using System.Collections.Generic;
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
        
        public IEnumerable<NavigationTableEntry> GetNavigationTable()
        {
            return _navigationTableRepository.GetAll();
        }

        public NavigationTableEntry GetElementById(int id)
        {
            Require.Positive(id, nameof(id));

            return _navigationTableRepository.GetById(id);
        }

        public int CreateElement(NavigationTableEntry element)
        {
            Require.NotNull(element, nameof(element));
            
            return _navigationTableRepository.Create(element);
        }

        public void UpdateElement(NavigationTableEntry element)
        {
            Require.NotNull(element, nameof(element));
            
            _navigationTableRepository.Update(element);
        }

        public void DeleteElement(int id)
        {
            Require.Positive(id, nameof(id));

            _navigationTableRepository.Delete(id);
        }
    }
}