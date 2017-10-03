using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<Page> GetPageByIdAsync(int pageId)
        {
            Require.Positive(pageId, nameof(pageId));

            return _pageRepository.GetByIdAsync(pageId);
        }

        public async Task<Page> GetPageByUrlAsync(string pageUrl)
        {
            Require.NotEmpty(pageUrl, nameof(pageUrl));
            
            if (pageUrl.Last() == '/')
                pageUrl = pageUrl.Remove(pageUrl.Length - 1);
            
            var pagesWithThisName = await _pageRepository.FindByAsync(c => c.Url == pageUrl);
            return pagesWithThisName.SingleOrDefault();
        }

        public async Task<PageTreeNode> GetTreeOfPages()
        {
            var tree = new PageTreeNode();
            var allPages = await _pageRepository.GetAll();
            foreach (var path in allPages.Select(p => p.Url))
            {
                tree.AddPath(path);
            }
            return tree;
        }

        public Task<IEnumerable<Page>> GetAllPagesAsync()
        {
            return _pageRepository.GetAll();
        }

        public async Task<int> CreatePageByAddressAsync(Page pageToCreate)
        {
            Require.NotNull(pageToCreate, nameof(pageToCreate));
            Require.NotEmpty(pageToCreate.Url, nameof(pageToCreate.Url));
            
            return await _pageRepository.CreateAsync(pageToCreate);
        }

        public async Task UpdatePageAsync(Page pageToUpadte)
        {
            Require.NotNull(pageToUpadte, nameof(pageToUpadte));

            await _pageRepository.UpdateAsync(pageToUpadte);
        }

        public async Task DeletePageAsync(int idPageToDelete)
        {
            Require.Positive(idPageToDelete, nameof(idPageToDelete));

            await _pageRepository.DeleteAsync(idPageToDelete);
        }
        
    }
}