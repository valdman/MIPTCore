using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Infrastructure;
using Journalist;
using Journalist.Extensions;

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

        public News GetNewsByUrl(string newsUrl)
        {
            Require.NotEmpty(newsUrl, nameof(newsUrl));
            
            if (newsUrl.Last() == '/')
                newsUrl = newsUrl.Remove(newsUrl.Length - 1);
            
            var newsWithThisUrl = _newsRepository.FindBy(c => c.FullPageUri == newsUrl);
            return newsWithThisUrl.SingleOrDefault();
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
            MustBeValidNews(newsToCreate);

            return _newsRepository.Create(newsToCreate);
        }

        public void UpdateNews(News newsToCreate)
        {
            Require.NotNull(newsToCreate, nameof(newsToCreate));
            MustBeValidNews(newsToCreate);

            _newsRepository.Update(newsToCreate);
        }

        public void DeleteNews(int newsId)
        {
            Require.Positive(newsId, nameof(newsId));

            _newsRepository.Delete(newsId);
        }

        void MustBeValidNews(News news)
        {
            var newWithSameUrl = _newsRepository.FindBy(n => n.FullPageUri == news.FullPageUri).ToList();
            
            if(newWithSameUrl.IsEmpty())
                return;
            
            if(newWithSameUrl.Count() == 1 && newWithSameUrl.Single().Id != news.Id)
                throw new ArgumentException("Duplicate url");
        }
    }
}