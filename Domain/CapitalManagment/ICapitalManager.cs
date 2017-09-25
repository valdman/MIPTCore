﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CapitalManagment
{
    public interface ICapitalManager
    {
        Task<Capital> GetCapitalByIdAsync(int capitalId);
        Task<IEnumerable<Capital>> GetAllCapitalsAsync();
        Task<IEnumerable<Capital>> GetCapitalsByPredicateAsync(Expression<Func<Capital, bool>> predicate);

        Volume GetFundVolumeCapital();
        Task<int> CreateCapitalAsync(Capital capitalToCreate);
        Task UpdateCapitalAsync(Capital capitalToUpdate);
        Task DeleteCapitalAsync(int capitalToDeleteId);
    }
}