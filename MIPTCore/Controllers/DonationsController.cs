using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CapitalManagment;
using Common;
using DonationManagment;
using DonationManagment.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            var targetCapital = _capitalManager.GetCapitalById(comboModel.CapitalId);
            if (targetCapital == null)
                return NotFound("CapitalNotFound");

            var existingUser = _userManager.GetUserByEmail(comboModel.Email);

            int userIntendedToDonateId;

            if (existingUser == null)
            {
                var userToCreate = Mapper.Map<User>(comboModel);
                
                userToCreate.Password = new Password
                (
                    //crutches.js
                    Guid.NewGuid().ToString("n").Substring(0, 10)
                );
                userToCreate.Role = UserRole.User;
                
                userIntendedToDonateId = _userManager.CreateUser(userToCreate);

                await _userMailerService.BeginPasswordSettingAndEmailVerification(userIntendedToDonateId);
            }
            else
            {
                userIntendedToDonateId = existingUser.Id;
            }

            var donationToCreate = Mapper.Map<CreateDonationModel>(comboModel);
            donationToCreate.UserId = userIntendedToDonateId;

            return CreateDonation(donationToCreate, isAutocompleted: false);
        }

        // GET: donations
        [HttpGet, FormatFilter]
        [AllowAnonymous]
        public IActionResult GetAllDonations([FromQuery] PaginationAndFilteringParams filteringParams, string format, bool isPaginationDisabled)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<Donation> donationsToReturn;
            int total = -1;
            
            if (isPaginationDisabled)
            {
                donationsToReturn = User.IsInRole("Admin") 
                    ? _donationManager.GetAllDonations()
                    : _donationManager.GetDonationsByPredicate(donation => donation.IsConfirmed);
            }
            else
            {
                try
                {
                    var donationsPage = User.IsInRole("Admin") 
                        ? _donationManager.GetPaginated(filteringParams)
                        : _donationManager.GetPaginated(filteringParams, donation => donation.IsConfirmed);
    
                    donationsToReturn = donationsPage.Docs;
                    total = donationsPage.Total;
                }
                catch (System.Linq.Dynamic.Core.Exceptions.ParseException)
                {
                    return BadRequest("Ivalid filtering parameters");
                }
            }
            

            var expandedDonationModels = donationsToReturn.Select(donation => new ExpandedDonationModel
                {
                    Capital = Mapper.Map<CapitalModel>(_capitalManager.GetCapitalById(donation.CapitalId)),
                    User = Mapper.Map<UserModel>(_userManager.GetUserById(donation.UserId)),
                    Value = donation.Value,
                    IsConfirmed = donation.IsConfirmed,
                    IsRecursive = donation.IsRecursive,
                    CreatingTime = donation.CreatingTime,
                    Id = donation.Id
                })
                .ToList();

            if (isPaginationDisabled)
                return Ok(expandedDonationModels);
            
            var paginated = new PaginatedList<ExpandedDonationModel>(filteringParams, expandedDonationModels, total);
            return Ok(paginated);
        }

        // GET donations/5
        [HttpGet("{id}.{format?}"), FormatFilter]
        public IActionResult GetDonationbyId(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            if(!int.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var donationToReturn = _donationManager.GetDonationById(objectId);

            if (donationToReturn == null)
                return NotFound();

            return Ok(Mapper.Map<CreateDonationModel>(donationToReturn));
        }

        // POST donations
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateDonation([FromBody]CreateDonationModel donationModel,[FromQuery]bool isAutocompleted)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (isAutocompleted && !User.IsInRole("Admin"))
                return Unauthorized();

            var targetCapital = _capitalManager.GetCapitalById(donationModel.CapitalId);
            if (targetCapital == null)
                return NotFound("CapitalNotFound");

            var donationToCreate = Mapper.Map<Donation>(donationModel);

            return !isAutocompleted ?
                Ok(_donationManager.CreateDonationAsync(donationToCreate)) :
                Ok(_donationManager.CreateCompletedSingleDonation(donationToCreate));
        }

        // PUT donations/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]UpdateDonationModel donationModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!int.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldDonation = _donationManager.GetDonationById(objectId);

            if (oldDonation == null)
                return NotFound();

            oldDonation.UserId = donationModel.UserId;
            oldDonation.CapitalId = donationModel.CapitalId;
            oldDonation.Value = donationModel.Value;
            oldDonation.IsRecursive = donationModel.IsRecursive;
            oldDonation.PaymentType = donationModel.PaymentType;

            _donationManager.UpdateDonation(oldDonation);
            return Ok(id);
        }

        // DELETE donations/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if(!int.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldDonation = _donationManager.GetDonationById(objectId);

            if (oldDonation == null)
                return NotFound();

            _donationManager.DeleteDonation(objectId);
            return Ok(id);
        }
    }
}
