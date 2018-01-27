using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public override Story GetById(int id)
        {
            Require.Positive(id, nameof(id));

            return FindBy(news => news.Id == id).SingleOrDefault();
        }

        public override IEnumerable<Story> GetAll()
        {
            return Db
                .Include(c => c.Owner).ThenInclude(o => o.Image)
                .Where(@object => !@object.IsDeleted)
                .ToList();
        }

        public override IEnumerable<Story> FindBy(Expression<Func<Story, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            return Db
                .Include(c => c.Owner).ThenInclude(o => o.Image)
                .Where(predicate)
                .Where(@object => !@object.IsDeleted)
                .ToList();
        }
    }
}