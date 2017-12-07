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
    }
}