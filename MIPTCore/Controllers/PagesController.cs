using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Extensions;
using MIPTCore.Models;
using MIPTCore.Models.ComplexMappers;
using PagesManagment;

namespace MIPTCore.Controllers
{
    [Route("[controller]")]
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
        public async Task<IActionResult> GetTree()
        {
            var pageTree = await _pagesManager.GetTreeOfPages();
            
            return Ok(PageTreeMapper.PageTreeToModel(pageTree));
        }
        
        // GET pages
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allPages = await _pagesManager.GetAllPagesAsync();

            return Ok(allPages.Select(Mapper.Map<PageModel>));
        }

        // GET pages/5 or pages/page/url
        [HttpGet("{*pageIndex}")]
        public async Task<IActionResult> Get(string pageIndex)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Page pageToReturn;
            if (int.TryParse(pageIndex, out var pageId))
            {
                this.CheckIdViaModel(pageId);
                pageToReturn = await _pagesManager.GetPageByIdAsync(pageId);
            }
            else
            {
               pageToReturn = await _pagesManager.GetPageByUrlAsync(pageIndex);
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
        public async Task<IActionResult> Post([FromBody] PageCreationModel pageModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pageToCreate = Mapper.Map<Page>(pageModel);

            var pageId = await _pagesManager.CreatePageByAddressAsync(pageToCreate);

            return Ok(pageId);
        }

        // PUT pages/5
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] PageUpdateModel pageModel)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pageToUpdate = await _pagesManager.GetPageByIdAsync(id);

            if (pageToUpdate == null)
            {
                return NotFound("Page not found");
            }

            //updation
            pageToUpdate.Name = pageModel.Name;
            pageToUpdate.Url = pageModel.Url;
            pageToUpdate.Description = pageModel.Description;
            pageToUpdate.Content = pageModel.Content;
            
            await _pagesManager.UpdatePageAsync(pageToUpdate);
            
            //!!!
            var updatedPage = await _pagesManager.GetPageByIdAsync(id);

            return Ok(Mapper.Map<PageModel>(updatedPage));
        }

        // DELETE pages/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var pageToDelete = await _pagesManager.GetPageByIdAsync(id);
            
            if (pageToDelete == null)
            {
                return NotFound("User not found");
            }

            await _pagesManager.DeletePageAsync(id);

            return Ok();
        }
    }
}