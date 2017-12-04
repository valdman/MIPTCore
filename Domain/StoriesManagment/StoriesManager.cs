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

        public Story GetStoryById(int storyId)
        {
            Require.Positive(storyId, nameof(storyId));
            
            return _storiesRepository.GetById(storyId);
        }

        public IEnumerable<Story> GetAllStories()
        {
            return _storiesRepository.GetAll();
        }

        public IEnumerable<Story> GetStoriesByPredicate(Expression<Func<Story, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            return _storiesRepository.FindBy(predicate);
        }

        public int CreateStory(Story storyToCreate)
        {
            Require.NotNull(storyToCreate, nameof(storyToCreate));
            
            return _storiesRepository.Create(storyToCreate);
        }

        public void UpdateStory(Story storyToUpdate)
        {
            Require.NotNull(storyToUpdate, nameof(storyToUpdate));
            
            _storiesRepository.Update(storyToUpdate);
        }

        public void DeleteStory(int storyId)
        {
            Require.Positive(storyId, nameof(storyId));
            
            _storiesRepository.Delete(storyId);
        }
    }
}