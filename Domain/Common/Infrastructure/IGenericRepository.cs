using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.Entities.Entities.ReadModifiers;
using Common.ReadModifiers;

namespace Common.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {

        T GetById(int id);
        IEnumerable<T> GetAll();
        IQueryable<T> AsQueryable();
        (int, IEnumerable<T>) GetAllForPagination(PaginationParams paginationParams, OrderingParams orderingParams, IEnumerable<FilteringParams> filteringParams, Expression<Func<T, bool>> predicate = null);
        IEnumerable<T> GetWithFiltersAndOrder(IEnumerable<FilteringParams> filteringParams, OrderingParams orderingParams);
        
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        int Create(T @object);
        void Delete(int objectId);
        void Update(T @object);
    }
}