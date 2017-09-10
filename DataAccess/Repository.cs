using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Journalist;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class Repository<T> : IRepository<T> where T : PersistentEntity
    {
        private readonly DbSessionProvider<T> _sessionProvider;

        public Repository(DbSessionProvider<T> sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }
        
        public async Task<T> GetByIdAsync(int objectToGetId)
        {
            Require.Positive(objectToGetId, nameof(objectToGetId));
            
            using (var sessionContext = _sessionProvider.GetEntityContext())
            {
                return await sessionContext.DbSet.FindAsync(objectToGetId);
            }
        }

        public async Task<IEnumerable<T>> GetByPredicateAsync(Expression<Func<T, bool>> predicate = null)
        {
            using (var sessionContext = _sessionProvider.GetEntityContext())
            {
                return predicate == null ? 
                    await sessionContext.DbSet.ToListAsync() : 
                    await sessionContext.DbSet.Where(predicate).ToListAsync();
            }  
        }

        public async Task<int> CreateAsync(T objectToCreate)
        {
            using (var sessionContext = _sessionProvider.GetEntityContext())
            {
                await sessionContext.DbSet.AddAsync(objectToCreate);
                await sessionContext.SaveChangesAsync();
                return objectToCreate.Id;
            }
        }

        public async Task UpdateAsync(T objectToUpdate)
        {
            using (var sessionContext = _sessionProvider.GetEntityContext())
            {
                await UpdateByValues(objectToUpdate, sessionContext);
                await sessionContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int objectToDeleteId)
        {
            using (var sessionContext = _sessionProvider.GetEntityContext())
            {
                var @object = await GetByIdAsync(objectToDeleteId);
                if (@object == null)
                {
                    return;
                }
                @object.IsDeleted = true;
                await UpdateAsync(@object);
            }
        }

        private async Task UpdateByValues(T item, DbSessionProvider<T>.EntityContext<T> context)
        {
            var collection = context.DbSet;
            var entity = await collection.FindAsync(item.Id);
            if (entity == null)
            {
                return;
            }

            context.Entry(entity).CurrentValues.SetValues(item);
        }
    }
}