using System.Linq;
using AutoMapper;
using Common.Entities;
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
        public IActionResult Get()
        {
            var allNews = _newsManager.GetAllNews().OrderByDescending(n => n.CreatingTime);

            return Ok(allNews.Select(Mapper.Map<NewsModel>));
        }

        // GET news/5 or news/page/url
        [HttpGet("{*newsIndex}")]
        public IActionResult Get(string newsIndex)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            News newsToReturn;
            if (int.TryParse(newsIndex, out var newsId))
            {
                this.CheckIdViaModel(newsId);
                newsToReturn = _newsManager.GetNewsById(newsId);
            }
            else
            {
                newsToReturn = _newsManager.GetNewsByUrl(newsIndex);
            }

            if (newsToReturn == null)
            {
                return NotFound("News with this ID or Name is not exists");
            }
            
            return Ok(Mapper.Map<NewsModel>(newsToReturn));
        }
        
        // POST news
        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Post([FromBody] NewsCreationModel newsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newsToCreate = Mapper.Map<News>(newsModel);

            var pageId = _newsManager.CreateNews(newsToCreate);

            return Ok(pageId);
        }

        // PUT news/5
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public IActionResult Put(int id, [FromBody] NewsUpdateModel newsModel)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newsToUpdate = _newsManager.GetNewsById(id);

            if (newsToUpdate == null)
            {
                return NotFound("News not found");
            }

            //updation
            var newImage = Mapper.Map<Image>(newsModel.Image);
            
            newsToUpdate.Name = newsModel.Name;
            newsToUpdate.FullPageUri = newsModel.FullPageUri;
            newsToUpdate.Content = newsModel.Content;
            newsToUpdate.Description = newsModel.Description;
            newsToUpdate.Date = newsModel.Date;
            newsToUpdate.Image = newImage;

            _newsManager.UpdateNews(newsToUpdate);
            
            //!!!
            var updatedNews = _newsManager.GetNewsById(id);

            return Ok(Mapper.Map<NewsModel>(updatedNews));
        }

        // DELETE news/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public IActionResult Delete(int id)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var pageToDelete = _newsManager.GetNewsById(id);
            
            if (pageToDelete == null)
            {
                return NotFound("News not found");
            }

            _newsManager.DeleteNews(id);

            return Ok();
        }
    }
}