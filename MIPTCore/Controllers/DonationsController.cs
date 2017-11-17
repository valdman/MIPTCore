using System;
using DonationManagment.Application;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MGSUCore.Models.Mappers;
using MGSUCore.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CapitalManagment;
using ProjectManagment.Application;
using UserManagment.Application;
using Common;
using DonationManagment;
using Newtonsoft.Json;
using MGSUCore.Models.Convertors;
using MIPTCore.Models;
using UserManagment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MGSUCore.Controllers
{

    [Authorize("Admin")]
    [Route("[controller]")]
    public class DonationsController : Controller
    {
        private readonly IDonationManager _donationManager;
        private readonly ICapitalManager _capitalManager;
        private readonly IUserManager _userManager;

        public DonationsController(IDonationManager donationManager, ICapitalManager capitalManager, IUserManager userManager)
        {
            _donationManager = donationManager;
            _capitalManager = capitalManager;
            _userManager = userManager;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> ComboDonation(DonationWithRegistrationModel comboModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToCreate = Mapper.Map<User>(comboModel);
            //простите меня
            userToCreate.Password = new Password
            (
                //crutches.js
                Guid.NewGuid().ToString("n").Substring(0, 10)
            );
            userToCreate.Role = UserRole.User;

            var newuserId = await _userManager.CreateUserAsync(userToCreate);

            var donationToCreate = Mapper.Map<SaveDonationModel>(comboModel);

            return CreateDonation(donationToCreate);
        }

        // GET: api/values
        [HttpGet]
        [AllowAnonymous]
        public async Task<OkObjectResult> GetAllDonations()
        {
            IEnumerable<Donation> donationsToReturn;
            if(User.IsInRole("Admin"))
            {
                donationsToReturn = await _donationManager.GetDonationsByPredicate();
            }
            else
            {
                donationsToReturn = await _donationManager.GetDonationsByPredicate(donation => donation.IsConfirmed);
            }

            var expandedDonationModels = new List<ExpandedDonationModel>();
            foreach (var donation in donationsToReturn)
            {
                expandedDonationModels.Add(new ExpandedDonationModel
                {
                    Capital = ProjectMapper.ProjectToProjectModel(
                                    _capitalManager.GetProjectById(donation.ProjectId)),
                    User = UserMapper.UserToUserModel(
                                    _userManager.GetUserById(donation.UserId)),
                    Value = donation.Value,
                    Date = donation.Date,
                    Confirmed = donation.Confirmed,
                    Recursive = donation.Recursive,
                    CreatingDate = donation.CreatingDate,
                    Id = donation.Id
                });
            }

            return Ok(expandedDonationModels);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetDonationbyId(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            if(!int.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var donationToReturn = _donationManager.GetDonation(objectId);

            if (donationToReturn == null)
                return NotFound();

            return Ok(Mapper.Map<SaveDonationModel>(donationToReturn));
        }

        // POST api/values
        [HttpPost]
        public IActionResult CreateDonation([FromBody]SaveDonationModel donationModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var postToCreate = Mapper.Map<Donation>(donationModel);

            return Ok(_donationManager.CreateDonation(postToCreate).ToString());
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]SaveDonationModel donationModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!int.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldDonation = await _donationManager.GetDonation(objectId);

            if (oldDonation == null)
                return NotFound();

            oldDonation.UserId = donationModel.UserId;
            oldDonation.CapitalId = donationModel.CapitalId;
            oldDonation.Value = donationModel.Value;
            oldDonation.Date = donationModel.Date;
            oldDonation.IsRecursive = donationModel.Recursive;
            oldDonation.IsConfirmed = donationModel.Confirmed;

            await _donationManager.UpdateDonationAsync(oldDonation);
            return Ok(id);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if(!int.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldDonation = _donationManager.GetDonation(objectId);

            if (oldDonation == null)
                return NotFound();

            _donationManager.DeleteDonation(objectId);
            return Ok(id);
        }
    }
}
