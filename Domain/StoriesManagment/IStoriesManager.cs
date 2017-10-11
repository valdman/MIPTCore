using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StoriesManagment
{
    public interface IStoriesManager
    {
        Task<Story> GetStoryByIdAsync(int storyId);
        Task<IEnumerable<Story>> GetAllStoriesAsync();
        Task<IEnumerable<Story>> GetStoriesByPredicateAsync(Expression<Func<Story, bool>> predicate);

        Task<int> CreateStoryAsync(Story storyToCreate);
        Task UpdateStoryAsync(Story storyToUpdate);
        Task DeleteStoryAsync(int storyId);
    }
}