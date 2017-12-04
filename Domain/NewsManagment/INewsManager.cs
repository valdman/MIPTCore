using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NewsManagment
{
    public interface INewsManager
    {
        News GetNewsById(int newsId);
        IEnumerable<News> GetAllNews();
        IEnumerable<News> GetNewsByPredicate(Expression<Func<News, bool>> predicate);

        int CreateNews(News newsToCreate);
        void UpdateNews(News newsToCreate);
        void DeleteNews(int newsId);
    }
}