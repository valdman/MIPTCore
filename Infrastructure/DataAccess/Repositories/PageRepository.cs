using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;
using PagesManagment;
using PagesManagment.Infrastructure;

namespace DataAccess.Repositories
{
    public class PageRepository : GenericRepository<Page>, IPageRepository
    {
        public PageRepository(PageContext context) : base(context)
        {
            Db = context.Pages;
        }
        
        public override async Task<Page> GetByIdAsync(int id)
        {
            Require.Positive(id, nameof(id));
            
            var foundedObject = await Db.FindAsync(id);
            if (foundedObject == null || !foundedObject.IsDeleted)
            {
                return foundedObject;
            }
            return await Task.FromResult<Page>(null);
        }
        
        protected override DbSet<Page> Db { get; }
    }
}