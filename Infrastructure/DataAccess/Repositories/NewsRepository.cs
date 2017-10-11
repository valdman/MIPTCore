using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;
using NavigationHelper;
using NewsManagment;

namespace DataAccess.Repositories
{
    public class NewsRepository : GenericRepository<News>
    {
        public NewsRepository(WithImageContext context) : base(context)
        {
        }

        public override async Task<News> GetByIdAsync(int id)
        {
            Require.Positive(id, nameof(id));

            return (await FindByAsync(news => news.Id == id)).SingleOrDefault();
        }

        public override async Task<IEnumerable<News>> GetAll()
        {
            return await Db.Include(c => c.Image)
                .Where(@object => !@object.IsDeleted)
                .ToListAsync();
        }

        public override async Task<IEnumerable<News>> FindByAsync(Expression<Func<News, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            return await Db.Include(c => c.Image).Where(predicate)
                .Where(@object => !@object.IsDeleted)
                .ToListAsync();
        }
    }
}