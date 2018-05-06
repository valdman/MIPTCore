using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Extensions;
using MIPTCore.Models;
using MIPTCore.Models.ComplexMappers;
using PagesManagment;

namespace MIPTCore.Controllers
{
    [Route("api/[controller]")]
    public class PagesController : Controller
    {
        private readonly IPageManager _pagesManager;

        public PagesController(IPageManager pagesManager)
        {
            _pagesManager = pagesManager;
        }

        // GET pages/tree
        [HttpGet]
        [Route("tree")]
        public IActionResult GetTree()
        {
            var pageTree = _pagesManager.GetTreeOfPages();
            
            return Ok(PageTreeMapper.PageTreeToModel(pageTree));
        }
        
        // GET pages
        [HttpGet]
        public IActionResult Get()
        {
            var allPages = _pagesManager.GetAllPages();

            return Ok(allPages.Select(Mapper.Map<PageModel>));
        }

        // GET pages/5 or pages/page/url
        [HttpGet("{*pageIndex}")]
        public IActionResult Get(string pageIndex)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Page pageToReturn;
            if (int.TryParse(pageIndex, out var pageId))
            {
                this.CheckIdViaModel(pageId);
                pageToReturn = _pagesManager.GetPageById(pageId);
            }
            else
            {
               pageToReturn = _pagesManager.GetPageByUrl(pageIndex);
            }

            if (pageToReturn == null)
            {
                return NotFound("Page with this ID or Name is not exists");
            }
            
            return Ok(Mapper.Map<PageModel>(pageToReturn));
        }
        
        // POST pages
        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Post([FromBody] PageCreationModel pageModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pageToCreate = Mapper.Map<Page>(pageModel);

            var pageId = _pagesManager.CreatePageByAddress(pageToCreate);

            return Ok(pageId);
        }

        // PUT pages/5
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public IActionResult Put(int id, [FromBody] PageUpdateModel pageModel)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pageToUpdate = _pagesManager.GetPageById(id);

            if (pageToUpdate == null)
            {
                return NotFound("Page not found");
            }

            //updation
            pageToUpdate.Name = pageModel.Name;
            pageToUpdate.Url = pageModel.Url;
            pageToUpdate.Description = pageModel.Description;
            pageToUpdate.Content = pageModel.Content;
            
            _pagesManager.UpdatePage(pageToUpdate);
            
            //!!!
            var updatedPage = _pagesManager.GetPageById(id);

            return Ok(Mapper.Map<PageModel>(updatedPage));
        }

        // DELETE pages/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public IActionResult Delete(int id)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var pageToDelete = _pagesManager.GetPageById(id);
            
            if (pageToDelete == null)
            {
                return NotFound("User not found");
            }

            _pagesManager.DeletePage(id);

            return Ok();
        }
    }
}