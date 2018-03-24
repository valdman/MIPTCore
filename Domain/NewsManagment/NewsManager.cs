using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.DomainSteroids;
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

        public News GetNewsById(int newsId, bool includeHidden = false)
        {
            Require.Positive(newsId, nameof(newsId));
            
            var news = _newsRepository.GetById(newsId);

            return ShouldIncludeHidden(includeHidden).Compile().Invoke(news) 
                ? news 
                : null;
        }

        public News GetNewsByUrl(string newsUrl, bool includeHidden = false)
        {
            Require.NotEmpty(newsUrl, nameof(newsUrl));
            
            if (newsUrl.Last() == '/')
                newsUrl = newsUrl.Remove(newsUrl.Length - 1);
            
            var newsWithThisUrl = _newsRepository
                .FindBy(
                    ShouldIncludeHidden(includeHidden)
                        .And(c => c.FullPageUri == newsUrl));
            return newsWithThisUrl.SingleOrDefault();
        }

        public IEnumerable<News> GetAllNews(bool includeHidden = false)
        {
            return _newsRepository.FindBy(ShouldIncludeHidden(includeHidden));
        }

        public IEnumerable<News> GetNewsByPredicate(Expression<Func<News, bool>> predicate, bool includeHidden = false)
        {
            return _newsRepository.FindBy(predicate.And(ShouldIncludeHidden(includeHidden)));
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

        private Expression<Func<News, bool>> ShouldIncludeHidden(bool includeHidden) =>
            news => news.IsVisible || includeHidden;

        private void MustBeValidNews(News news)
        {
            var newWithSameUrl = _newsRepository.FindBy(n => n.FullPageUri == news.FullPageUri).ToList();
            
            if(newWithSameUrl.IsEmpty())
                return;
            
            if(newWithSameUrl.Count() == 1 && newWithSameUrl.Single().Id != news.Id)
                throw new ArgumentException("Duplicate url");
        }
    }
}