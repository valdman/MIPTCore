using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IRepository<T> where T : PersistentEntity
    {
        Task<T> GetByIdAsync(int objectToGetId);
        Task<IEnumerable<T>> GetByPredicateAsync(Expression<Func<T, bool>> predicate = null);

        Task<int> CreateAsync(T objectToCreate);
        Task UpdateAsync(T objectToUpdate);
        Task DeleteAsync(int objectToDeleteId);
    }
}