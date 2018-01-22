using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {

        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(PaginationAndFilteringParams filteringParams);
        
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, PaginationAndFilteringParams filteringParams);
        int Create(T @object);
        void Delete(int objectId);
        void Update(T @object);
    }
}