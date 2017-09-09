using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess
{
    public class Repository<T> : IRepository<T> where T : PersistentEntity
    {
        
        public T GetById(int objectToGetId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetByPredicate(Expression<Func<bool, T>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public int Create(T objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(T objectToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int objectToDeleteId)
        {
            throw new NotImplementedException();
        }
    }
}