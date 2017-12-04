using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StoriesManagment
{
    public interface IStoriesManager
    {
        Story GetStoryById(int storyId);
        IEnumerable<Story> GetAllStories();
        IEnumerable<Story> GetStoriesByPredicate(Expression<Func<Story, bool>> predicate);

        int CreateStory(Story storyToCreate);
        void UpdateStory(Story storyToUpdate);
        void DeleteStory(int storyId);
    }
}