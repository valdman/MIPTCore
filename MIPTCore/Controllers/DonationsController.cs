using System;
using DonationManagment.Application;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MGSUCore.Models;
using MGSUCore.Models.Mappers;
using MGSUCore.Filters;
using System.Collections.Generic;
using System.Linq;
using ProjectManagment.Application;
using UserManagment.Application;
using Common;
using Newtonsoft.Json;
using MGSUCore.Models.Convertors;
using UserManagment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MGSUCore.Controllers
{

    [Authorize("Admin")]
    [Route("[controller]")]
    public class DonationsController : Controller
    {
        private readonly IDonationManager _donationManager;
        private readonly IProjectManager _projectManager;
        private readonly IUserManager _userManager;

        public DonationsController(IDonationManager donationManager, IProjectManager projectManager, IUserManager userManager)
        {
            _donationManager = donationManager;
            _projectManager = projectManager;
            _userManager = userManager;
        }

        [HttpPost("registration")]
        public IActionResult ComboDonation(DonationWithRegistrationModel comboModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToCreate = new User
            {
                FirstName = comboModel.FirstName,
                LastName = comboModel.LastName,
                Email = comboModel.Email,
                IsEmailConfirmed = comboModel.Confirmed,
                //простите меня
                Password = new Password
                (
                    //crutches.js
                    Guid.NewGuid().ToString("n").Substring(0, 10)
                ),
                Role = UserRole.User
            };

            var newuserId = _userManager.CreateUser(userToCreate);

            var donationToCreate = new SaveDonationModel
            {
                UserId = newuserId,
                ProjectId = comboModel.ProjectId,
                Value = comboModel.Value,
                Date = comboModel.Date,
                Recursive = comboModel.Recursive,
                Confirmed = comboModel.Confirmed
            };

            return CreateDonation(donationToCreate);
        }

        // GET: api/values
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllDonations()
        {
            IEnumerable<Donation> donationsToReturn;
            if(User.IsInRole("Admin"))
            {
                donationsToReturn = _donationManager.GetDonationsByPredicate();
            }
            else
            {
                donationsToReturn = _donationManager.GetDonationsByPredicate(donation => donation.Confirmed);
            }

            var expandedDonationModels = new List<ExpandedDonationModel>();
            foreach (var donation in donationsToReturn)
            {
                expandedDonationModels.Add(new ExpandedDonationModel
                {
                    Project = ProjectMapper.ProjectToProjectModel(
                                    _projectManager.GetProjectById(donation.ProjectId)),
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
                
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var donationToReturn = _donationManager.GetDonation(objectId);

            if (donationToReturn == null)
                return NotFound();

            return Ok(DonationMapper.DonationToDonationModel(donationToReturn));
        }

        // POST api/values
        [HttpPost]
        public IActionResult CreateDonation([FromBody]SaveDonationModel donationModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var postToCreate = DonationMapper.DonationModelToDonation(donationModel);

            return Ok(_donationManager.CreateDonation(postToCreate).ToString());
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]SaveDonationModel donationModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldDonation = _donationManager.GetDonation(objectId);

            if (oldDonation == null)
                return NotFound();

            oldDonation.UserId = donationModel.UserId == ObjectId.Empty ? oldDonation.UserId : donationModel.UserId;
            oldDonation.ProjectId = donationModel.ProjectId == ObjectId.Empty ? oldDonation.ProjectId : donationModel.ProjectId;
            oldDonation.Value = donationModel.Value == 0 ? oldDonation.Value : donationModel.Value;
            oldDonation.Date = donationModel.Date;
            oldDonation.Recursive = donationModel.Recursive;
            oldDonation.Confirmed = donationModel.Confirmed;

            _donationManager.UpdateDonation(oldDonation);
            return Ok(id);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldDonation = _donationManager.GetDonation(objectId);

            if (oldDonation == null)
                return NotFound();

            _donationManager.DeleteDonation(objectId);
            return Ok(id);
        }
    }
}
