using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CapitalsTableHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Extensions;
using MIPTCore.Models;
using NavigationHelper;

namespace MIPTCore.Controllers
{
    public class FrontendHelpersController : Controller
    {
        private readonly ICapitalsTableHelper _capitalsTableHelper;
        private readonly INavigationHelper _navigationHelper;

        public FrontendHelpersController(ICapitalsTableHelper capitalsTableHelper, INavigationHelper navigationHelper)
        {
            _capitalsTableHelper = capitalsTableHelper;
            _navigationHelper = navigationHelper;
        }

        [HttpGet("capitals-layout")]
        public async Task<IActionResult> GetProjectsTable()
        {
            var capitalsTable = await _capitalsTableHelper.GetTableForCapitals();
            return Ok(capitalsTable.Select(Mapper.Map<CapitalsTableEntryModel>));
        }
        
        [HttpPut("capitals-layout")]
        [Authorize("Admin")]
        public async Task<IActionResult> CreateWholeCapitalsTable([FromBody] IEnumerable<CapitalsTableEntryModel> capitalsTable)
        {
            var catitalTableToCreate = capitalsTable.Select(Mapper.Map<CapitalsTableEntry>);
            
            await _capitalsTableHelper.SaveTable(catitalTableToCreate);
            return Ok();
        }
        
        [HttpGet("navigation-layout")]
        public async Task<IActionResult> GetNavigationTable()
        {
            var navigationTable = await _navigationHelper.GetNavigationTable();

            var orderedTable = navigationTable.OrderBy(element => element.Position);
            return Ok(orderedTable.Select(Mapper.Map<NavigationTableEntryModel>));
        }
        
        [HttpGet("navigation-layout/{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> GetNavigationElement(int id)
        {
            this.CheckIdViaModel(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var elementToReturn = await _navigationHelper.GetElementById(id);

            if (elementToReturn == null)
                return NotFound("Navigation element not found");
            
            return Ok(Mapper.Map<NavigationTableEntryModel>(elementToReturn));
        }
        
        [HttpPost("navigation-layout")]
        [Authorize("Admin")]
        public async Task<IActionResult> CreateNavigationElement([FromBody] NavigationTableEntryModel navigationTableElement)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var navigationElements = Mapper.Map<NavigationTableEntry>(navigationTableElement);
            
            var id = await _navigationHelper.CreateElement(navigationElements);
            return Ok(id);
        }
        
        [HttpPut("navigation-layout/{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> UpdateNavigationElement(int id, [FromBody] NavigationTableEntryModel navigationTableElement)
        {
            this.CheckIdViaModel(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var elementToUpdate = await _navigationHelper.GetElementById(id);

            if (elementToUpdate == null)
                return BadRequest("Navigation element not found");

            elementToUpdate.Name = navigationTableElement.Name;
            elementToUpdate.Position = navigationTableElement.Position;
            elementToUpdate.Url = navigationTableElement.Url;

            await _navigationHelper.UpdateElement(elementToUpdate);
            
            var updated = await _navigationHelper.GetElementById(id);
            
            return Ok(Mapper.Map<NavigationTableEntryModel>(updated));
        }
        
        [HttpDelete("navigation-layout/{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> DeleteNavigationElement(int id)
        {
            this.CheckIdViaModel(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var elementToDelete = await _navigationHelper.GetElementById(id);

            if (elementToDelete == null)
                return NotFound("Navigation element not found");

            await _navigationHelper.DeleteElement(id);
            return Ok(id);
        }
    }
}