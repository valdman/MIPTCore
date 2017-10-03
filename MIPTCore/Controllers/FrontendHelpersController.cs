using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CapitalsTableHelper;
using Common.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Models;

namespace MIPTCore.Controllers
{
    [Route("[controller]")]
    public class FrontendHelpersController : Controller
    {
        private readonly ICapitalsTableHelper _capitalsTableHelper;

        public FrontendHelpersController(ICapitalsTableHelper capitalsTableHelper)
        {
            _capitalsTableHelper = capitalsTableHelper;
        }

        [HttpGet("capitalsTable")]
        public async Task<IActionResult> GetProjectsTable()
        {
            var capitalsTable = await _capitalsTableHelper.GetTableForCapitals();
            return Ok(capitalsTable.Select(Mapper.Map<CapitalsTableEntryModel>));
        }
        
        [HttpPut("capitalsTable")]
        [Authorize("Admin")]
        public async Task<IActionResult> CreateWholeTable([FromBody] IEnumerable<CapitalsTableEntryModel> capitalsTable)
        {
            var catitalTableToCreate = capitalsTable.Select(Mapper.Map<CapitalsTableEntry>);
            
            await _capitalsTableHelper.SaveTable(catitalTableToCreate);
            return Ok();
        }
    }
}