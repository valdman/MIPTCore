using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CapitalManagment;
using Common;
using DonationManagment;
using DonationManagment.Application;
using Journalist.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Authentification;
using MIPTCore.Models;
using UserManagment;
using UserManagment.Application;

namespace MIPTCore.Controllers
{

    [Authorize("Admin")]
    [Route("[controller]")]
    public class DonationsController : Controller
    {
        private readonly IDonationManager _donationManager;
        private readonly ICapitalManager _capitalManager;
        private readonly IUserManager _userManager;
        private readonly IUserMailerService _userMailerService;

        public DonationsController(IDonationManager donationManager, ICapitalManager capitalManager, IUserManager userManager, IUserMailerService userMailerService)
        {
            _donationManager = donationManager;
            _capitalManager = capitalManager;
            _userManager = userManager;
            _userMailerService = userMailerService;
        }

        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IActionResult> ComboDonation([FromBody]DonationWithRegistrationModel comboModel)
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
            await _userMailerService.BeginPasswordSettingAndEmailVerification(newuserId);

            var donationToCreate = Mapper.Map<CreateDonationModel>(comboModel);

            return await CreateDonation(donationToCreate, isAutocompleted: false);
        }

        // GET: donations
        [HttpGet]
        [AllowAnonymous]
        public async Task<OkObjectResult> GetAllDonations()
        {
            IEnumerable<Donation> donationsToReturn;
            if(User.IsInRole("Admin"))
            {
                donationsToReturn = await _donationManager.GetAllDonations();
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
                    Capital = Mapper.Map<CapitalModel>(
                                    await _capitalManager.GetCapitalByIdAsync(donation.CapitalId)),
                    User = Mapper.Map<UserModel>(
                                    await _userManager.GetUserByIdAsync(donation.UserId)),
                    Value = donation.Value,
                    IsConfirmed = donation.IsConfirmed,
                    IsRecursive = donation.IsRecursive,
                    CreatingTime = donation.CreatingTime,
                    Id = donation.Id
                });
            }

            return Ok(expandedDonationModels);
        }

        // GET donations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonationbyId(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            if(!int.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var donationToReturn = await _donationManager.GetDonationByIdAsync(objectId);

            if (donationToReturn == null)
                return NotFound();

            return Ok(Mapper.Map<CreateDonationModel>(donationToReturn));
        }

        // POST donations
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateDonation([FromBody]CreateDonationModel donationModel,[FromQuery]bool isAutocompleted)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (isAutocompleted && !User.IsInRole("Admin"))
                return Unauthorized();

            var donationToCreate = Mapper.Map<Donation>(donationModel);

            return !isAutocompleted ?
                Ok(await _donationManager.CreateDonationAsync(donationToCreate)) :
                Ok(await _donationManager.CreateCompletedSingleDonation(donationToCreate));
        }

        // PUT donations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]UpdateDonationModel donationModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!int.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldDonation = await _donationManager.GetDonationByIdAsync(objectId);

            if (oldDonation == null)
                return NotFound();

            oldDonation.UserId = donationModel.UserId;
            oldDonation.CapitalId = donationModel.CapitalId;
            oldDonation.Value = donationModel.Value;
            oldDonation.IsRecursive = donationModel.IsRecursive;

            await _donationManager.UpdateDonationAsync(oldDonation);
            return Ok(id);
        }

        // DELETE donations/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if(!int.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldDonation = _donationManager.GetDonationByIdAsync(objectId);

            if (oldDonation == null)
                return NotFound();

            _donationManager.DeleteDonation(objectId);
            return Ok(id);
        }
    }
}
