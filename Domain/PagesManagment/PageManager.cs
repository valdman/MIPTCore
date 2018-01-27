using System.Collections.Generic;
using System.Linq;
using Journalist;
using PagesManagment.Infrastructure;

namespace PagesManagment
{
    public class PageManager : IPageManager
    {
        private readonly IPageRepository _pageRepository;

        public PageManager(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public Page GetPageById(int pageId)
        {
            Require.Positive(pageId, nameof(pageId));

            return _pageRepository.GetById(pageId);
        }

        public Page GetPageByUrl(string pageUrl)
        {
            Require.NotEmpty(pageUrl, nameof(pageUrl));
            
            if (pageUrl.Last() == '/')
                pageUrl = pageUrl.Remove(pageUrl.Length - 1);
            
            var pagesWithThisName = _pageRepository.FindBy(c => c.Url == pageUrl);
            return pagesWithThisName.SingleOrDefault();
        }

        public PageTreeNode GetTreeOfPages()
        {
            var tree = new PageTreeNode();
            var allPages = _pageRepository.GetAll();
            foreach (var path in allPages.Select(p => new {p.Url, p.Id}))
            {
                tree.AddPath(path.Id, path.Url);
            }
            return tree;
        }

        public IEnumerable<Page> GetAllPages()
        {
            return _pageRepository.GetAll();
        }

        public int CreatePageByAddress(Page pageToCreate)
        {
            Require.NotNull(pageToCreate, nameof(pageToCreate));
            Require.NotEmpty(pageToCreate.Url, nameof(pageToCreate.Url));
            
            return _pageRepository.Create(pageToCreate);
        }

        public void UpdatePage(Page pageToUpadte)
        {
            Require.NotNull(pageToUpadte, nameof(pageToUpadte));

            _pageRepository.Update(pageToUpadte);
        }

        public void DeletePage(int idPageToDelete)
        {
            Require.Positive(idPageToDelete, nameof(idPageToDelete));

            _pageRepository.Delete(idPageToDelete);
        }
        
    }
}