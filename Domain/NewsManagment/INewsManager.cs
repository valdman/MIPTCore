using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NewsManagment
{
    public interface INewsManager
    {
        News GetNewsById(int newsId, bool includeHidden = false);
        News GetNewsByUrl(string newsUrl, bool includeHidden = false);
        IEnumerable<News> GetAllNews(bool includeHidden = false);
        IEnumerable<News> GetNewsByPredicate(Expression<Func<News, bool>> predicate, bool includeHidden = false);

        int CreateNews(News newsToCreate);
        void UpdateNews(News newsToCreate);
        void DeleteNews(int newsId);
    }
}