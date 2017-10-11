using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Infrastructure;
using Journalist;

namespace StoriesManagment
{
    public class StoriesManager : IStoriesManager
    {
        private readonly IGenericRepository<Story> _storiesRepository;

        public StoriesManager(IGenericRepository<Story> storiesRepository)
        {
            _storiesRepository = storiesRepository;
        }

        public Task<Story> GetStoryByIdAsync(int storyId)
        {
            Require.Positive(storyId, nameof(storyId));
            
            return _storiesRepository.GetByIdAsync(storyId);
        }

        public Task<IEnumerable<Story>> GetAllStoriesAsync()
        {
            return _storiesRepository.GetAll();
        }

        public Task<IEnumerable<Story>> GetStoriesByPredicateAsync(Expression<Func<Story, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            return _storiesRepository.FindByAsync(predicate);
        }

        public Task<int> CreateStoryAsync(Story storyToCreate)
        {
            Require.NotNull(storyToCreate, nameof(storyToCreate));
            
            return _storiesRepository.CreateAsync(storyToCreate);
        }

        public Task UpdateStoryAsync(Story storyToUpdate)
        {
            Require.NotNull(storyToUpdate, nameof(storyToUpdate));
            
            return _storiesRepository.UpdateAsync(storyToUpdate);
        }

        public Task DeleteStoryAsync(int storyId)
        {
            Require.Positive(storyId, nameof(storyId));
            
            return _storiesRepository.DeleteAsync(storyId);
        }
    }
}