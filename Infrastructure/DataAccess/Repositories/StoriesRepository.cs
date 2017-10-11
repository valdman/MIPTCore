using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Infrastructure;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;
using StoriesManagment;

namespace DataAccess.Repositories
{
    public class StoriesRepository : GenericRepository<Story>
    {
        public StoriesRepository(WithImageContext context) : base(context)
        {
        }

        public override async Task<Story> GetByIdAsync(int id)
        {
            Require.Positive(id, nameof(id));

            return (await FindByAsync(news => news.Id == id)).SingleOrDefault();
        }

        public override async Task<IEnumerable<Story>> GetAll()
        {
            return await Db
                .Include(c => c.Owner).ThenInclude(o => o.Image)
                .Where(@object => !@object.IsDeleted)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Story>> FindByAsync(Expression<Func<Story, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            return await Db
                .Include(c => c.Owner).ThenInclude(o => o.Image)
                .Where(predicate)
                .Where(@object => !@object.IsDeleted)
                .ToListAsync();
        }
    }
}