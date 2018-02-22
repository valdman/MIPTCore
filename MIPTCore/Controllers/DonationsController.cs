using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CapitalManagment;
using Common.Entities;
using Common.Entities.Entities.ReadModifiers;
using Common.ReadModifiers;
using DonationManagment;
using DonationManagment.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Models;
using Newtonsoft.Json;
using System.Xml.Serialization;
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
        [HttpGet]
        public IActionResult GetAllDonations([FromQuery] string filteringParamsString,
                                            [FromQuery] PaginationParams paginationParams,
                                            [FromQuery] OrderingParams orderingParams,
                                            bool isPaginationDisabled,
                                            bool download)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var filteringParams = ParseFilterParamsString(filteringParamsString);

            IEnumerable<Donation> donationsToReturn;
            int total = -1;

            try
            {
                if (isPaginationDisabled)
                {
                    donationsToReturn = _donationManager.GetWithFiltersAndOrder(filteringParams, orderingParams);
                    var expandedDonationToReturn = ExpandDonations(donationsToReturn).ToList();
                    if (download)
                        return DumpDonationsInXml(expandedDonationToReturn);
                    return Ok(expandedDonationToReturn);
                }

                var donationsPage =
                    _donationManager.GetPaginatedDonations(paginationParams, orderingParams, filteringParams);

                donationsToReturn = donationsPage.Docs;
                total = donationsPage.Total;
            }
            catch (System.Linq.Dynamic.ParseException e)
            {
                return BadRequest(e.ToString());
            }
            catch (Exception)
            {
                return BadRequest("An error has occured");
            }

            var expandedDonationModels = ExpandDonations(donationsToReturn);

            var paginated = new PaginatedList<ExpandedDonationModel>(paginationParams, expandedDonationModels, total);

            if (download)
                return DumpDonationsInXml(paginated);
            return Ok(paginated);
        }

        // GET donations/5
        [HttpGet("{id:int}")]
        public IActionResult GetDonationbyId(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var donationToReturn = _donationManager.GetDonationById(id);

            if (donationToReturn == null)
                return NotFound();

            return Ok(Mapper.Map<ShortDonationModel>(donationToReturn));
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
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]UpdateDonationModel donationModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var oldDonation = _donationManager.GetDonationById(id);

            if (oldDonation == null)
                return NotFound();

            oldDonation.UserId = donationModel.UserId;
            oldDonation.CapitalId = donationModel.CapitalId;
            oldDonation.Value = donationModel.Value;
            oldDonation.IsRecursive = donationModel.IsRecursive;
            oldDonation.IsConfirmed = donationModel.IsConfirmed;
            oldDonation.PaymentType = donationModel.PaymentType;

            _donationManager.UpdateDonation(oldDonation);
            return Ok(id);
        }

        // DELETE donations/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {

            var oldDonation = _donationManager.GetDonationById(id);

            if (oldDonation == null)
                return NotFound();

            _donationManager.DeleteDonation(id);
            return Ok(id);
        }

        private IEnumerable<ExpandedDonationModel> ExpandDonations(IEnumerable<Donation> donationsToReturn)
        {
            var donationsList = donationsToReturn as IList<Donation> ?? donationsToReturn.ToList();
            
            var capitalToJoinIds = donationsList.Select(d => d.CapitalId).ToHashSet();
            var userToJoinIds = donationsList.Select(d => d.UserId).ToHashSet();

            var capitalsToJoin = _capitalManager.GetCapitalsByPredicate(c => capitalToJoinIds.Any(i => i.Equals(c.Id))).ToList();
            var usersToJoin = _userManager.GetUsersByPredicate(u => userToJoinIds.Any(i => i.Equals(u.Id))).ToList();

            foreach (var donation in donationsList)
            {
                yield return new ExpandedDonationModel
                {
                    Capital = Mapper.Map<ShortCapitalModel>(capitalsToJoin.SingleOrDefault(c => c.Id == donation.CapitalId)),
                    User = Mapper.Map<UserModel>(usersToJoin.SingleOrDefault(u => u.Id == donation.UserId)),
                    Value = donation.Value,
                    IsConfirmed = donation.IsConfirmed,
                    IsRecursive = donation.IsRecursive,
                    CreatingTime = donation.CreatingTime,
                    PaymentType = donation.PaymentType,
                    Id = donation.Id
                };
            }
        }

        private IEnumerable<FilteringParams> ParseFilterParamsString(string filteringParamsString)
        {
            IEnumerable<FilteringParams> filteringParams;
            try
            {
                filteringParams =
                    (FilteringParams[]) JsonConvert.DeserializeObject(filteringParamsString, typeof(FilteringParams[]));
            }
            catch (Exception e)
            {
                filteringParams = new FilteringParams[0];
            }
            return filteringParams;
        }

        private FileStreamResult DumpDonationsInXml(object expandedDonationModels)
        {
            var serializer = new XmlSerializer(expandedDonationModels.GetType());

            var xmlStream = new MemoryStream();

            serializer.Serialize(xmlStream, expandedDonationModels);
            xmlStream.Seek(0, 0);

            return new FileStreamResult(xmlStream, "application/octet-stream");
        }
    }
}
