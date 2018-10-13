using System.Linq;
using AutoMapper;
using DataAccess.Contexts;
using DonationManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MIPTCore.Authentification;
using MIPTCore.Models;
using UserManagment;

namespace MIPTCore.Controllers
{
    [Authorize("User")]
    [Route("api/me")]
    public class AccountServiceController : Controller
    {
        //Read directly from context when its just Query request
        private readonly DbSet<User> _userSet;
        private readonly DbSet<Donation> _donationsSet;

        public AccountServiceController(UserContext userContext, WithImageContext withImageContext)
        {
            _donationsSet = withImageContext.Set<Donation>();
            _userSet = userContext.Set<User>();
        }

        // GET me
        public IActionResult Current()
        {
            var currentUserId = User.GetId();
            var currentUser = _userSet.FirstOrDefault(u => u.Id == currentUserId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            return Ok(Mapper.Map<UserModel>(currentUser));
        }


        //GET me/donations
        [HttpGet("dontations")]
        public IActionResult GetUserDonations()
        {
            var currentUserId = User.GetId();
            var userDonations = _donationsSet
                .AsQueryable()
                .Where(d => !d.IsDeleted && d.IsConfirmed && d.UserId == currentUserId)
                .AsEnumerable();

            return Ok(userDonations.Select(Mapper.Map<DonationRelatedToUserModdel>));
        }
    }
}