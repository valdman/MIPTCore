using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagesManagment
{
    public interface IPageManager
    {
        Task<Page> GetPageByIdAsync(int pageId); 
        Task<Page> GetPageByUrlAsync(string pageUrl);
        Task<PageTreeNode> GetTreeOfPages();
        Task<IEnumerable<Page>> GetAllPagesAsync();
        
        Task<int> CreatePageByAddressAsync(Page pageToCreate);
        
        Task UpdatePageAsync(Page pageToUpadte);
        Task DeletePageAsync(int idPageToDelete);
    }
}