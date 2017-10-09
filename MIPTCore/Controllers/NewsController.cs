using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Extensions;
using MIPTCore.Models;
using NewsManagment;

namespace MIPTCore.Controllers
{
    [Route("[controller]")]
    public class NewsController : Controller
    {
        private readonly INewsManager _newsManager;

        public NewsController(INewsManager newsManager)
        {
            _newsManager = newsManager;
        }
        
        // GET news
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allNews = await _newsManager.GetAllNewsAsync();

            return Ok(allNews.Select(Mapper.Map<NewsModel>));
        }

        // GET news/5
        [HttpGet("{newsId}")]
        public async Task<IActionResult> Get(int newsId)
        {   
            this.CheckIdViaModel(newsId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newsToReturn = await _newsManager.GetNewsByIdAsync(newsId);

            if (newsToReturn == null)
            {
                return NotFound("News with this ID is not exists");
            }
            
            return Ok(Mapper.Map<NewsModel>(newsToReturn));
        }
        
        // POST news
        [HttpPost]
        [Authorize("Admin")]
        public async Task<IActionResult> Post([FromBody] NewsCreationModel newsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newsToCreate = Mapper.Map<News>(newsModel);

            var pageId = await _newsManager.CreateNewsAsync(newsToCreate);

            return Ok(pageId);
        }

        // PUT news/5
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] NewsUpdateModel newsModel)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newsToUpdate = await _newsManager.GetNewsByIdAsync(id);

            if (newsToUpdate == null)
            {
                return NotFound("News not found");
            }

            //updation
            var newImage = Mapper.Map<Image>(newsModel.Image);
            
            newsToUpdate.Name = newsModel.Name;
            newsToUpdate.Content = newsModel.Content;
            newsToUpdate.Description = newsModel.Description;
            newsToUpdate.Date = newsModel.Date;
            newsToUpdate.Image = newImage;

            await _newsManager.UpdateNewsAsync(newsToUpdate);
            
            //!!!
            var updatedNews = await _newsManager.GetNewsByIdAsync(id);

            return Ok(Mapper.Map<NewsModel>(updatedNews));
        }

        // DELETE news/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var pageToDelete = await _newsManager.GetNewsByIdAsync(id);
            
            if (pageToDelete == null)
            {
                return NotFound("News not found");
            }

            await _newsManager.DeleteNewsAsync(id);

            return Ok();
        }
    }
}