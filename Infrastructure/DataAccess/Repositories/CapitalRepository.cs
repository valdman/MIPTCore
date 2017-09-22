using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CapitalManagment;
using Common;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CapitalRepository : GenericRepository<Capital>
    {
        public override async Task<Capital> GetByIdAsync(int id)
        {
            Require.Positive(id, nameof(id));

            return await Db.Include(u => u.Image).Where(u => u.Id == id).SingleOrDefaultAsync();
        }

        public override async Task<IEnumerable<Capital>> GetAll()
        {
            return await Db.Include(u => u.Image).ToListAsync();
        }

        public override async Task<IEnumerable<Capital>> FindByAsync(Expression<Func<Capital, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));

            return await Db.Include(u => u.Image).Where(predicate).ToListAsync();
        }

        public CapitalRepository(CapitalContext context) : base(context)
        {
            Db = context.Capitals;
        }

        protected override DbSet<Capital> Db { get; }
    }
}