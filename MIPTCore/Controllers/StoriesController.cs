﻿using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CapitalManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Extensions;
using MIPTCore.Models;
using StoriesManagment;

namespace MIPTCore.Controllers
{
    [Route("[controller]")]
    public class StoriesController : Controller
    {
        private readonly IStoriesManager _storiesManager;

        public StoriesController(IStoriesManager storiesManager)
        {
            _storiesManager = storiesManager;
        }

        // GET stories
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allStories = await _storiesManager.GetAllStoriesAsync();

            return Ok(allStories.Select(Mapper.Map<StoryModel>));
        }

        // GET stories/5
        [HttpGet("{storyId}")]
        public async Task<IActionResult> Get(int storyId)
        {   
            this.CheckIdViaModel(storyId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storyToReturn = await _storiesManager.GetStoryByIdAsync(storyId);

            if (storyToReturn == null)
            {
                return NotFound("Story with this ID is not exists");
            }
            
            return Ok(Mapper.Map<StoryModel>(storyToReturn));
        }
        
        // POST stories
        [HttpPost]
        [Authorize("Admin")]
        public async Task<IActionResult> Post([FromBody] StoryCreationModel storyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storyToCreate = Mapper.Map<Story>(storyModel);

            var storyId = await _storiesManager.CreateStoryAsync(storyToCreate);

            return Ok(storyId);
        }

        // PUT stories/5
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] StoryUpdateModel storyModel)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storyToUpdate = await _storiesManager.GetStoryByIdAsync(id);

            if (storyToUpdate == null)
            {
                return NotFound("Story not found");
            }

            //updation
            var newStoryOwner = Mapper.Map<Person>(storyModel.Owner);

            storyToUpdate.Owner = newStoryOwner;
            storyToUpdate.Content = storyModel.Content;

            await _storiesManager.UpdateStoryAsync(storyToUpdate);
            
            //!!!
            var updatedStory = await _storiesManager.GetStoryByIdAsync(id);

            return Ok(Mapper.Map<StoryModel>(updatedStory));
        }

        // DELETE stories/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storyToDelete = await _storiesManager.GetStoryByIdAsync(id);
            
            if (storyToDelete == null)
            {
                return NotFound("Story not found");
            }

            await _storiesManager.DeleteStoryAsync(id);

            return Ok();
        }
    }
}