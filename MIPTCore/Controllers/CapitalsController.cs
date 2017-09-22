﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CapitalManagment;
using Common;
using Common.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Extensions;
using MIPTCore.Models;

namespace MIPTCore.Controllers
{
    [Route("[controller]")]
    public class CapitalsController : Controller
    {
        private readonly ICapitalManager _capitalManager;

        public CapitalsController(ICapitalManager capitalManager)
        {
            _capitalManager = capitalManager;
        }

        // GET capitals
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var capitalsToReturn = await _capitalManager.GetAllCapitalsAsync();
            return Ok(capitalsToReturn.Select(Mapper.Map<CapitalModel>));
        }
        
        // GET capitals/5
        [HttpGet("{capitalId}")]
        public async Task<IActionResult> Get(int capitalId)
        {
            this.CheckIdViaModel(capitalId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var capitalToReturn = await _capitalManager.GetCapitalByIdAsync(capitalId);

            if (capitalToReturn == null)
            {
                return NotFound("Capital with this ID is not exists");
            }
            
            return Ok(Mapper.Map<CapitalModel>(capitalToReturn));
        }
        
        // POST capitals
        [HttpPost]
        [Authorize("Admin")]
        public async Task<IActionResult> Post([FromBody] CapitalCreatingModel capitalModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var capitalToCreate = Mapper.Map<Capital>(capitalModel);

            var capitalId = await _capitalManager.CreateCapitalAsync(capitalToCreate);

            return Ok(capitalId);
        }
        
        // PUT capitals/5
        [HttpPut("{capitalId}")]
        [Authorize("Admin")]
        public async Task<IActionResult> Put(int capitalId, [FromBody] CapitalUpdatingModel capitalModel)
        {
            this.CheckIdViaModel(capitalId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var capitalToUpdate = await _capitalManager.GetCapitalByIdAsync(capitalId);
            
            if (capitalToUpdate == null)
            {
                return NotFound("Capital not found");
            }

            capitalToUpdate.Name = capitalModel.Name;
            capitalToUpdate.Need = capitalModel.Need;
            capitalToUpdate.Description = capitalModel.Description;
            capitalToUpdate.Image = Mapper.Map<Image>(capitalModel.Image);
            capitalToUpdate.Founders = Mapper.Map<IEnumerable<Person>>(capitalModel.Founders);
            capitalToUpdate.Recivers = Mapper.Map<IEnumerable<Person>>(capitalModel.Recivers);

            await _capitalManager.UpdateCapitalAsync(capitalToUpdate);

            return Ok(Mapper.Map<CapitalModel>(capitalToUpdate));
        }
        
        // DELETE capitals/5
        [HttpDelete("{capitalId}")]
        [Authorize("Admin")]
        public async Task<IActionResult> Delete(int capitalId)
        {
            this.CheckIdViaModel(capitalId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var capitalToDelete = await _capitalManager.GetCapitalByIdAsync(capitalId);
            
            if (capitalToDelete == null)
            {
                return NotFound("Capital not found");
            }

            await _capitalManager.DeleteCapitalAsync(capitalId);

            return Ok();
        }

    }
}