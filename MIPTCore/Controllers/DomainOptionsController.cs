using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Models;

namespace MIPTCore.Controllers
{
    [Authorize("Admin")]
    [Route("[controller]")]
    public class DomainOptionsController : Controller
    {
        private readonly IDomainOptionsService _domainOptionsService;

        public DomainOptionsController(IDomainOptionsService domainOptionsService)
        {
            _domainOptionsService = domainOptionsService;
        }

        [HttpGet]
        public IActionResult GetDomainOptions()
        {
            return Ok(_domainOptionsService.GetDomainOptions());
        }

        [HttpPut]
        public IActionResult UpdateDomainOptions([FromBody] DomainOptionsUpdateModel newOptions)
        {
            var oldOptions = _domainOptionsService.GetDomainOptions();
            if (!decimal.TryParse(newOptions.SizeOfFund, out var newSize))
                return BadRequest("Ivalid fund size");
            
            oldOptions.SizeOfFund = newSize;
            _domainOptionsService.UpdateDomainOption(oldOptions);
            return Ok();
        }
    }
}