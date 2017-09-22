﻿using System;
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

            _capitalContext.TryUpdateManyToMany(model.Founders, founders ,a => a.Id);
            _capitalContext.TryUpdateManyToMany(model.Recivers, recievers,a => a.Id);
            
            Db.Update(@object);
            await Save();
        }

        public CapitalRepository(CapitalContext context) : base(context)
        {
            Db = context.Capitals;
            _capitalContext = context;
        }

        protected override DbSet<Capital> Db { get; }
        private readonly CapitalContext _capitalContext;
    }
}