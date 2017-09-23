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
    public abstract class GenericRepository<TEntity> :
        IGenericRepository<TEntity> where TEntity : PersistentEntity
    {
        private readonly DbContext _context;
        protected abstract DbSet<TEntity> Db { get; }

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            Require.Positive(id, nameof(id));
            
            var foundedObject = await _context.Set<TEntity>().FindAsync(id);
            if (foundedObject == null || !foundedObject.IsDeleted)
            {
                return foundedObject;
            }
            return await Task.FromResult<TEntity>(null);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Db.Where(@object => !@object.IsDeleted).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            return await Db.Where(predicate).ToListAsync();
        }

        public virtual async Task<int> CreateAsync(TEntity @object)
        {
            Require.NotNull(@object, nameof(@object));
            
            @object.CreatingTime = DateTimeOffset.Now;
            _context.Set<TEntity>().Add(@object);
            await Save();
            return @object.Id;
        }

        public virtual async Task DeleteAsync(int objectId)
        {
            Require.Positive(objectId, nameof(objectId));
            
            var objectToDelete = await GetByIdAsync(objectId);
            objectToDelete.IsDeleted = true;
            await UpdateAsync(objectToDelete);
            await Save();
        }

        public virtual async Task UpdateAsync(TEntity @object)
        {
            Require.NotNull(@object, nameof(@object));
            
            var entity = await GetByIdAsync(@object.Id);
            if (entity == null)
            {
                return;
            }

            _context.Update(@object);
            await Save();
        }

        protected virtual async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}