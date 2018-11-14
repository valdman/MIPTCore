using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapitalManagment;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Extensions;
using MIPTCore.Models;

namespace MIPTCore.Controllers
{
    [Route("api/[controller]")]
    public class CapitalsController : Controller
    {
        private readonly ICapitalManager _capitalManager;

        public CapitalsController(ICapitalManager capitalManager)
        {
            _capitalManager = capitalManager;
        }

        // GET capitals
        [HttpGet]
        public IActionResult Get()
        {
            var capitalsToReturn = _capitalManager.GetAllCapitals();
            return Ok(capitalsToReturn.Select(Mapper.Map<CapitalModel>));
        }
        
        // GET capitals/volume
        [HttpGet]
        [Route("volume")]
        public IActionResult GetVolume()
        {
            var fundVolume = _capitalManager.GetFundVolumeCapital();
            return Ok(fundVolume);
        }
        
        // GET capitals/5 or capitals/nameOfCapital
        [HttpGet("{*capitalIndex}")]
        public IActionResult Get(string capitalIndex, [FromQuery]bool withCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Capital capitalToReturn;
            if (int.TryParse(capitalIndex, out var capitalId))
            {
                this.CheckIdViaModel(capitalId);
                capitalToReturn = _capitalManager.GetCapitalById(capitalId);
            }
            else
            {
                capitalToReturn = _capitalManager.GetCapitalByFullUri(capitalIndex);
            }
            
            if (capitalToReturn == null)
            {
                return NotFound("Capital with this ID or Name is not exists");
            }
            
            return Ok(User.IsInRole("Admin") && withCredentials 
                ? Mapper.Map<CapitalModelForAdmin>(capitalToReturn) 
                : Mapper.Map<CapitalModel>(capitalToReturn));
        }
        
        // POST capitals
        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Post([FromBody] CapitalCreatingModel capitalModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var capitalToCreate = Mapper.Map<Capital>(capitalModel);

            var capitalId = _capitalManager.CreateCapital(capitalToCreate);

            return Ok(capitalId);
        }
        
        // PUT capitals/5
        [HttpPut("{capitalId}")]
        [Authorize("Admin")]
        public IActionResult Put(int capitalId, [FromBody] CapitalUpdatingModel capitalModel)
        {
            this.CheckIdViaModel(capitalId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var capitalToUpdate = _capitalManager.GetCapitalById(capitalId);
            
            if (capitalToUpdate == null)
            {
                return NotFound("Capital not found");
            }

            capitalToUpdate.Name = capitalModel.Name;
            capitalToUpdate.Description = capitalModel.Description;
            capitalToUpdate.BankAccountInformation = capitalModel.BankAccountInformation;
            capitalToUpdate.OfferLink = capitalModel.OfferLink;
            capitalToUpdate.CapitalCredentials = Mapper.Map<CapitalCredentials>(capitalModel.CapitalCredentials);
            capitalToUpdate.Given = capitalModel.Given;
            capitalToUpdate.Image = Mapper.Map<Image>(capitalModel.Image);
            capitalToUpdate.Founders = Mapper.Map<IEnumerable<Person>>(capitalModel.Founders);
            capitalToUpdate.Recivers = Mapper.Map<IEnumerable<Person>>(capitalModel.Recivers);
            capitalToUpdate.Capitalizations = Mapper.Map<IEnumerable<Capitalization>>(capitalModel.Capitalizations);
            capitalToUpdate.FullPageUri = capitalModel.FullPageUri;
            capitalToUpdate.Content = capitalModel.Content;

            _capitalManager.UpdateCapital(capitalToUpdate);

            return Ok(Mapper.Map<CapitalModelForAdmin>(capitalToUpdate));
        }
        
        // DELETE capitals/5
        [HttpDelete("{capitalId}")]
        [Authorize("Admin")]
        public IActionResult Delete(int capitalId)
        {
            this.CheckIdViaModel(capitalId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var capitalToDelete = _capitalManager.GetCapitalById(capitalId);
            
            if (capitalToDelete == null)
            {
                return NotFound("Capital not found");
            }

            _capitalManager.DeleteCapital(capitalId);

            return Ok();
        }

    }
}