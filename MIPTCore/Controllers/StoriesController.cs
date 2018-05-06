using System.Linq;
using AutoMapper;
using CapitalManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Extensions;
using MIPTCore.Models;
using StoriesManagment;

namespace MIPTCore.Controllers
{
    [Route("api/[controller]")]
    public class StoriesController : Controller
    {
        private readonly IStoriesManager _storiesManager;

        public StoriesController(IStoriesManager storiesManager)
        {
            _storiesManager = storiesManager;
        }

        // GET stories
        [HttpGet]
        public IActionResult Get()
        {
            var allStories = _storiesManager.GetAllStories().OrderByDescending(s => s.CreatingTime);

            return Ok(allStories.Select(Mapper.Map<StoryModel>));
        }

        // GET stories/5 or stories/page/url
        [HttpGet("{*storyIndex}")]
        public IActionResult Get(string storyIndex)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Story storyToReturn;
            if (int.TryParse(storyIndex, out var storyId))
            {
                this.CheckIdViaModel(storyId);
                storyToReturn = _storiesManager.GetStoryById(storyId);
            }
            else
            {
                storyToReturn = _storiesManager.GetStoryByUrl(storyIndex);
            }

            if (storyToReturn == null)
            {
                return NotFound("Story with this ID or Name is not exists");
            }
            
            return Ok(Mapper.Map<StoryModel>(storyToReturn));
        }
        
        // POST stories
        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Post([FromBody] StoryCreationModel storyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storyToCreate = Mapper.Map<Story>(storyModel);

            var storyId = _storiesManager.CreateStory(storyToCreate);

            return Ok(storyId);
        }

        // PUT stories/5
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public IActionResult Put(int id, [FromBody] StoryUpdateModel storyModel)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storyToUpdate = _storiesManager.GetStoryById(id);

            if (storyToUpdate == null)
            {
                return NotFound("Story not found");
            }

            //updation
            var newStoryOwner = Mapper.Map<Person>(storyModel.Owner);

            storyToUpdate.Owner = newStoryOwner;
            storyToUpdate.Content = storyModel.Content;
            storyToUpdate.FullPageUri = storyModel.FullPageUri;

            _storiesManager.UpdateStory(storyToUpdate);
            
            //!!!
            var updatedStory = _storiesManager.GetStoryById(id);

            return Ok(Mapper.Map<StoryModel>(updatedStory));
        }

        // DELETE stories/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public IActionResult Delete(int id)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storyToDelete = _storiesManager.GetStoryById(id);
            
            if (storyToDelete == null)
            {
                return NotFound("Story not found");
            }

            _storiesManager.DeleteStory(id);

            return Ok();
        }
    }
}