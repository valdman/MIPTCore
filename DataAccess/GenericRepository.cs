using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;
using Common.Infrastructure;
using Journalist;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class GenericRepository<T> :
        IGenericRepository<T> where T : PersistentEntity
    {
        private readonly DbSessionProvider _sessionProvider;
        private readonly DbSet<T> _db;

        public GenericRepository(DbSessionProvider sessionProvider)
        {   
            _sessionProvider = sessionProvider;
            _db = _sessionProvider.Set<T>();
        }

        public async Task<T> GetById(int id)
        {
            Require.Positive(id, nameof(id));
            
            var foundedObject = await _sessionProvider.Set<T>().FindAsync(id);
            if (foundedObject == null || !foundedObject.IsDeleted)
            {
                return foundedObject;
            }
            return await Task.FromResult<T>(null);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _db.Where(@object => !@object.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            return await _db.Where(predicate).ToListAsync();
        }

        public virtual async Task<int> CreateAsync(T @object)
        {
            Require.NotNull(@object, nameof(@object));
            
            @object.CreatedTime = DateTimeOffset.Now;
            await _sessionProvider.Set<T>().AddAsync(@object);
            await Save();
            return @object.Id;
        }

        public virtual async Task DeleteAsync(int objectId)
        {
            Require.Positive(objectId, nameof(objectId));
            
            var objectToDelete = await GetById(objectId);
            objectToDelete.IsDeleted = true;
            await UpdateAsync(objectToDelete);
            await Save();
        }

        public virtual async Task UpdateAsync(T @object)
        {
            Require.NotNull(@object, nameof(@object));
            
            var entity = await GetById(@object.Id);
            if (entity == null)
            {
                return;
            }

            _sessionProvider.Update(@object);
            await Save();
        }

        protected virtual async Task Save()
        {
            await _sessionProvider.SaveChangesAsync();
        }
    }
}