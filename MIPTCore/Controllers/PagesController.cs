﻿using System;
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
            throw new NotImplementedException();
        }

        // GET pages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var pageToReturn = await _pagesManager.GetPageByIdAsync(id);

            if (pageToReturn == null)
            {
                return NotFound("Page with this ID is not exists");
            }
            
            return Ok(Mapper.Map<PageModel>(pageToReturn));
        }

        // GET pages/pageuri
        [HttpGet("url/{*url}")]
        public async Task<IActionResult> Get(string url)
        {
            var pageToReturn = await _pagesManager.GetPageByUrlAsync(url);

            if (pageToReturn == null)
            {
                return NotFound("Page with this URL is not exists");
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