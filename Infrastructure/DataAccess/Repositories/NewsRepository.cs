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

        public override News GetById(int id)
        {
            Require.Positive(id, nameof(id));

            return FindBy(news => news.Id == id).SingleOrDefault();
        }

        public override IEnumerable<News> GetAll()
        {
            return Db
                .Include(c => c.Image)
                .Include(c => c.PreviewImage)
                .Where(@object => !@object.IsDeleted)
                .ToList();
        }

        public override IEnumerable<News> FindBy(Expression<Func<News, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            return Db
                .Include(c => c.Image)
                .Include(c => c.PreviewImage)
                .Where(predicate)
                .Where(@object => !@object.IsDeleted)
                .ToList();
        }
    }
}