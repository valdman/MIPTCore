using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CapitalsTableHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(navigationTable.Select(Mapper.Map<NavigationTableEntryModel>));
        }
        
        [HttpPut("navigation-layout")]
        [Authorize("Admin")]
        public async Task<IActionResult> CreateNavigationTable([FromBody] IEnumerable<NavigationTableEntryModel> navigationTable)
        {
            var navigationElements = navigationTable.Select(Mapper.Map<NavigationTableEntry>);
            
            await _navigationHelper.SaveTable(navigationElements);
            return Ok();
        }
    }
}