using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CapitalManagment;
using CapitalManagment.Infrastructure;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace DataAccess.Repositories
{
    public class CapitalRepository : GenericRepository<Capital>, ICapitalRepository
    {
        public override async Task<Capital> GetByIdAsync(int id)
        {
            Require.Positive(id, nameof(id));

            return await Db
                .Include(u => u.Image)
                .Include(u => u.Founders)
                .Include(u => u.Recivers)
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();
        }

        public override async Task<IEnumerable<Capital>> GetAll()
        {
            return await Db
                .Include(u => u.Image)
                .Include(u => u.Founders)
                .Include(u => u.Recivers)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Capital>> FindByAsync(Expression<Func<Capital, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));

            return await Db
                .Include(u => u.Image)
                .Include(u => u.Founders)
                .Include(u => u.Recivers)
                .Where(predicate)
                .ToListAsync();
        }

        public override async Task UpdateAsync(Capital @object)
        {
            Require.NotNull(@object, nameof(@object));

            var founders = @object.Founders?.ToArray();
            var recievers = @object.Recivers?.ToArray();
            
            var model = Db.SingleOrDefault(c => c.Id == @object.Id);
            if (model == null)
            {
                return;
            }

            Context.TryUpdateManyToMany(model.Founders, founders ,a => a.Id);
            Context.TryUpdateManyToMany(model.Recivers, recievers,a => a.Id);
            
            Db.Update(@object);
            await Save();
        }

        public async Task<Capital> GetCapitalByNameAsync(string name)
        {
            Require.NotEmpty(name, nameof(name));

            return await Db.SingleOrDefaultAsync(c => c.Name.Equals(name));
        }

        public decimal CoutSumGivenToWholeFund()
        {
            return Db.Sum(c => c.Given);
        }

        public CapitalRepository(CapitalContext context) : base(context)
        {
        }

    }
}