using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {

        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<int> Create(T @object);
        Task Delete(int objectId);
        Task UpdateAsync(T @object);
    }
}