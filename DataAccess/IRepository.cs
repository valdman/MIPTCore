using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess
{
    public interface IRepository<T> where T : PersistentEntity
    {
        T GetById(int objectToGetId);
        IEnumerable<T> GetByPredicate(Expression<Func<bool, T>> predicate = null);

        int Create(T objectToCreate);
        void Update(T objectToUpdate);
        void Delete(int objectToDeleteId);
    }
}