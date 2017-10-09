using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Infrastructure;
using Journalist;

namespace NewsManagment
{
    public class NewsManager : INewsManager
    {
        private readonly IGenericRepository<News> _newsRepository;

        public NewsManager(IGenericRepository<News> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public Task<News> GetNewsByIdAsync(int newsId)
        {
            Require.Positive(newsId, nameof(newsId));

            return _newsRepository.GetByIdAsync(newsId);
        }

        public Task<IEnumerable<News>> GetAllNewsAsync()
        {
            return _newsRepository.GetAll();
        }

        public Task<IEnumerable<News>> GetNewsByPredicateAsync(Expression<Func<News, bool>> predicate)
        {
            return _newsRepository.FindByAsync(predicate);
        }

        public Task<int> CreateNewsAsync(News newsToCreate)
        {
            Require.NotNull(newsToCreate, nameof(newsToCreate));

            return _newsRepository.CreateAsync(newsToCreate);
        }

        public Task UpdateNewsAsync(News newsToCreate)
        {
            Require.NotNull(newsToCreate, nameof(newsToCreate));

            return _newsRepository.UpdateAsync(newsToCreate);
        }

        public Task DeleteNewsAsync(int newsId)
        {
            Require.Positive(newsId, nameof(newsId));

            return _newsRepository.DeleteAsync(newsId);
        }
    }
}