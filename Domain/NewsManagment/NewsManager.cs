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

        public News GetNewsById(int newsId)
        {
            Require.Positive(newsId, nameof(newsId));

            return _newsRepository.GetById(newsId);
        }

        public IEnumerable<News> GetAllNews()
        {
            return _newsRepository.GetAll();
        }

        public IEnumerable<News> GetNewsByPredicate(Expression<Func<News, bool>> predicate)
        {
            return _newsRepository.FindBy(predicate);
        }

        public int CreateNews(News newsToCreate)
        {
            Require.NotNull(newsToCreate, nameof(newsToCreate));

            return _newsRepository.Create(newsToCreate);
        }

        public void UpdateNews(News newsToCreate)
        {
            Require.NotNull(newsToCreate, nameof(newsToCreate));

            _newsRepository.Update(newsToCreate);
        }

        public void DeleteNews(int newsId)
        {
            Require.Positive(newsId, nameof(newsId));

            _newsRepository.Delete(newsId);
        }
    }
}