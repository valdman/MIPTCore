using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagesManagment
{
    public interface IPageManager
    {
        Page GetPageById(int pageId); 
        Page GetPageByUrl(string pageUrl);
        PageTreeNode GetTreeOfPages();
        IEnumerable<Page> GetAllPages();
        
        int CreatePageByAddress(Page pageToCreate);
        
        void UpdatePage(Page pageToUpadte);
        void DeletePage(int idPageToDelete);
    }
}