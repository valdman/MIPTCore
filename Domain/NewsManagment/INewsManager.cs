using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NewsManagment
{
    public interface INewsManager
    {
        Task<News> GetNewsByIdAsync(int newsId);
        Task<IEnumerable<News>> GetAllNewsAsync();
        Task<IEnumerable<News>> GetNewsByPredicateAsync(Expression<Func<News, bool>> predicate);

        Task<int> CreateNewsAsync(News newsToCreate);
        Task UpdateNewsAsync(News newsToCreate);
        Task DeleteNewsAsync(int newsId);
    }
}