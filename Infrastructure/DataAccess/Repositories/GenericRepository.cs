using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;
using Common.Infrastructure;
using Journalist;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity> :
        IGenericRepository<TEntity> where TEntity : PersistentEntity
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> Db;

        protected GenericRepository(DbContext context)
        {
            Context = context;
            Db = Context.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            Require.Positive(id, nameof(id));
            
            var foundedObject = await Db.FindAsync(id);
            if (foundedObject == null || !foundedObject.IsDeleted)
            {
                return foundedObject;
            }
            return await Task.FromResult<TEntity>(null);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Db
                .Where(@object => !@object.IsDeleted)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            return await Db.Where(predicate)
                .Where(@object => !@object.IsDeleted)
                .ToListAsync();
        }

        public virtual async Task<int> CreateAsync(TEntity @object)
        {
            Require.NotNull(@object, nameof(@object));
            
            @object.CreatingTime = DateTimeOffset.Now;
            Db.Add(@object);
            
            await Save();
            return @object.Id;
        }

        public virtual async Task DeleteAsync(int objectId)
        {
            Require.Positive(objectId, nameof(objectId));
            
            var objectToDelete = await GetByIdAsync(objectId);
            objectToDelete.IsDeleted = true;
            
            await Save();
        }

        public virtual async Task UpdateAsync(TEntity @object)
        {
            Require.NotNull(@object, nameof(@object));

            await Save();
        }

        protected async Task Save()
        {
            await Context.SaveChangesAsync();
        }
    }
}