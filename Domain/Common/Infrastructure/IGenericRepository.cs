using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Entities.Entities.ReadModifiers;

namespace Common.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {

        T GetById(int id);
        IEnumerable<T> GetAll();
        (int, IEnumerable<T>) GetAllForPagination(PaginationParams paginationParams, OrderingParams orderingParams, FilteringParams filteringParams, Expression<Func<T, bool>> predicate = null);
        IEnumerable<T> GetWithFilterAndOrder(FilteringParams filteringParams, OrderingParams orderingParams);
        
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        int Create(T @object);
        void Delete(int objectId);
        void Update(T @object);
    }
}