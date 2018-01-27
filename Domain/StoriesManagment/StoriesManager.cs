using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.Infrastructure;
using Journalist;
using Journalist.Extensions;

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

        public Story GetStoryByUrl(string storyUrl)
        {
            Require.NotEmpty(storyUrl, nameof(storyUrl));
            
            if (storyUrl.Last() == '/')
                storyUrl = storyUrl.Remove(storyUrl.Length - 1);
            
            var newsWithThisUrl = _storiesRepository.FindBy(c => c.FullPageUri == storyUrl);
            return newsWithThisUrl.SingleOrDefault();
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
            MustBeValidStory(storyToCreate);
            
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
        
        void MustBeValidStory(Story story)
        {
            var newWithSameUrl = _storiesRepository.FindBy(n => n.FullPageUri == story.FullPageUri).ToList();
            
            if(newWithSameUrl.IsEmpty())
                return;
            
            if(newWithSameUrl.Count() == 1 && newWithSameUrl.Single().Id != story.Id)
                throw new ArgumentException("Duplicate url");
        }
    }
}