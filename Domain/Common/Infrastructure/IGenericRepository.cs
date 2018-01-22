﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {

        T GetById(int id);
        IEnumerable<T> GetAll();
        (int, IEnumerable<T>) GetAllForPagination(PaginationAndFilteringParams filteringParams, Expression<Func<T, bool>> predicate = null);
        
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        int Create(T @object);
        void Delete(int objectId);
        void Update(T @object);
    }
}