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
using NavigationHelper;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace DataAccess.Repositories
{
    public class CapitalRepository : GenericRepository<Capital>, ICapitalRepository
    {
        public override Capital GetById(int id)
        {
            Require.Positive(id, nameof(id));

            return Db
                .Include(u => u.CapitalCredentials)
                .Include(u => u.Image)
                    .Include(u => u.Founders)
                .ThenInclude(f => f.Image)
                    .Include(u => u.Recivers)
                .ThenInclude(r => r.Image)
                .SingleOrDefault(u => u.Id == id);
        }

        public override IEnumerable<Capital> GetAll()
        {
            return Db
                .Include(u => u.CapitalCredentials)
                .Include(u => u.Image)
                .Include(u => u.Founders)
                    .ThenInclude(f => f.Image)
                .Include(u => u.Recivers)
                    .ThenInclude(r => r.Image)
                .ToList();
        }

        public override IEnumerable<Capital> FindBy(Expression<Func<Capital, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));

            return Db
                .Include(u => u.CapitalCredentials)
                .Include(u => u.Image)
                .Include(u => u.Founders)
                    .ThenInclude(f => f.Image)
                .Include(u => u.Recivers)
                    .ThenInclude(r => r.Image)
                .Where(predicate)
                .ToList();
        }

        public override void Update(Capital @object)
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
            Save();
        }

        public Capital GetCapitalByFullUri(string name)
        {
            Require.NotEmpty(name, nameof(name));

            return Db.SingleOrDefault(c => c.FullPageUri.Equals(name));
        }

        public decimal CoutSumGivenToWholeFund()
        {
            return Db.Sum(c => c.Given);
        }

        public CapitalRepository(WithImageContext context) : base(context)
        {
        }

    }
}