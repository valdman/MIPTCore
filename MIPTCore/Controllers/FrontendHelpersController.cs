using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BannerHelper;
using CapitalsTableHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Extensions;
using MIPTCore.Models;
using NavigationHelper;

namespace MIPTCore.Controllers
{
    [Route("api")]
    public class FrontendHelpersController : Controller
    {
        private readonly ICapitalsTableHelper _capitalsTableHelper;
        private readonly INavigationHelper _navigationHelper;
        private readonly IBannerHelper _bannerHelper;

        public FrontendHelpersController(ICapitalsTableHelper capitalsTableHelper, INavigationHelper navigationHelper, IBannerHelper bannerHelper)
        {
            _capitalsTableHelper = capitalsTableHelper;
            _navigationHelper = navigationHelper;
            _bannerHelper = bannerHelper;
        }

        [HttpGet("capitals-layout")]
        public IActionResult GetProjectsTable()
        {
            var capitalsTable = _capitalsTableHelper.GetTableForCapitals();
            return Ok(capitalsTable.Select(Mapper.Map<CapitalsTableEntryModel>));
        }
        
        [HttpPut("capitals-layout")]
        [Authorize("Admin")]
        public IActionResult CreateWholeCapitalsTable([FromBody] IEnumerable<CapitalsTableEntryModel> capitalsTable)
        {
            var catitalTableToCreate = capitalsTable.Select(Mapper.Map<CapitalsTableEntry>);
            
            _capitalsTableHelper.SaveTable(catitalTableToCreate);
            return Ok();
        }
        
        [HttpGet("navigation-layout")]
        public IActionResult GetNavigationTable()
        {
            var navigationTable = _navigationHelper.GetNavigationTable();

            var orderedTable = navigationTable.OrderBy(element => element.Position);
            return Ok(orderedTable.Select(Mapper.Map<NavigationTableEntryModel>));
        }
        
        [HttpGet("navigation-layout/{id}")]
        [Authorize("Admin")]
        public IActionResult GetNavigationElement(int id)
        {
            this.CheckIdViaModel(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var elementToReturn = _navigationHelper.GetElementById(id);

            if (elementToReturn == null)
                return NotFound("Navigation element not found");
            
            return Ok(Mapper.Map<NavigationTableEntryModel>(elementToReturn));
        }
        
        [HttpPost("navigation-layout")]
        [Authorize("Admin")]
        public IActionResult CreateNavigationElement([FromBody] NavigationTableEntryModel navigationTableElement)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var navigationElements = Mapper.Map<NavigationTableEntry>(navigationTableElement);
            
            var id = _navigationHelper.CreateElement(navigationElements);
            return Ok(id);
        }
        
        [HttpPut("navigation-layout/{id}")]
        [Authorize("Admin")]
        public IActionResult UpdateNavigationElement(int id, [FromBody] NavigationTableEntryModel navigationTableElement)
        {
            this.CheckIdViaModel(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var elementToUpdate = _navigationHelper.GetElementById(id);

            if (elementToUpdate == null)
                return BadRequest("Navigation element not found");

            elementToUpdate.Name = navigationTableElement.Name;
            elementToUpdate.Position = navigationTableElement.Position;
            elementToUpdate.Url = navigationTableElement.Url;

            _navigationHelper.UpdateElement(elementToUpdate);
            
            var updated = _navigationHelper.GetElementById(id);
            
            return Ok(Mapper.Map<NavigationTableEntryModel>(updated));
        }
        
        [HttpDelete("navigation-layout/{id}")]
        [Authorize("Admin")]
        public IActionResult DeleteNavigationElement(int id)
        {
            this.CheckIdViaModel(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var elementToDelete = _navigationHelper.GetElementById(id);

            if (elementToDelete == null)
                return NotFound("Navigation element not found");

            _navigationHelper.DeleteElement(id);
            return Ok(id);
        }
        
        //new shit 
        
        [HttpGet("banner-layout")]
        public IActionResult GetBanner()
        {
            var banner = _bannerHelper.GetBanner();

            var orderedBanner = banner.OrderBy(element => element.Position);
            return Ok(orderedBanner.Select(Mapper.Map<BannerElementModel>));
        }
        
        [HttpGet("banner-layout/{id}")]
        [Authorize("Admin")]
        public IActionResult GetBannerElement(int id)
        {
            this.CheckIdViaModel(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var elementToReturn = _bannerHelper.GetElementById(id);

            if (elementToReturn == null)
                return NotFound("Banner element not found");
            
            return Ok(Mapper.Map<BannerElementModel>(elementToReturn));
        }
        
        [HttpPost("banner-layout")]
        [Authorize("Admin")]
        public IActionResult CreateBannerElement([FromBody] BannerElementModel bannerElementModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var bannerElement = Mapper.Map<BannerElement>(bannerElementModel);
            
            var id = _bannerHelper.CreateElement(bannerElement);
            return Ok(id);
        }
        
        [HttpPut("banner-layout/{id}")]
        [Authorize("Admin")]
        public IActionResult UpdateBannerElement(int id, [FromBody] BannerElementModel bannerElementModel)
        {
            this.CheckIdViaModel(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var elementToUpdate = _bannerHelper.GetElementById(id);

            if (elementToUpdate == null)
                return BadRequest("Navigation element not found");

            elementToUpdate.Name = bannerElementModel.Name;
            elementToUpdate.Position = bannerElementModel.Position;
            elementToUpdate.Url = bannerElementModel.Url;
            elementToUpdate.Type = bannerElementModel.Type;

            _bannerHelper.UpdateElement(elementToUpdate);
            
            var updated = _navigationHelper.GetElementById(id);
            
            return Ok(Mapper.Map<BannerElementModel>(updated));
        }
        
        [HttpDelete("banner-layout/{id}")]
        [Authorize("Admin")]
        public IActionResult DeleteBannerElement(int id)
        {
            this.CheckIdViaModel(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var elementToDelete = _bannerHelper.GetElementById(id);

            if (elementToDelete == null)
                return NotFound("Navigation element not found");

            _bannerHelper.DeleteElement(id);
            return Ok(id);
        }
    }
}