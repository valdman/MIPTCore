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
        }
    }
}