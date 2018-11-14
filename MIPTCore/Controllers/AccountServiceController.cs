using System;
using System.Linq;
using AutoMapper;
using CapitalManagment;
using Dapper;
using DataAccess.Contexts;
using DonationManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MIPTCore.Authentification;
using MIPTCore.Models;
using UserManagment;
using UserReadModel;

namespace MIPTCore.Controllers
{
    [Authorize("User")]
    [Route("api/me")]
    public class AccountServiceController : Controller
    {
        private readonly IUserAccountingReadModel _userAccountingReadModel;
        
        //Read directly from context when its just Query request
        private readonly DbSet<User> _userSet;

        public AccountServiceController(UserContext userContext, IUserAccountingReadModel userAccountingReadModel)
        {
            _userAccountingReadModel = userAccountingReadModel;
            _userSet = userContext.Set<User>();
        }

        // GET me
        public IActionResult Current()
        {
            var currentUserId = User.GetId();
            var currentUser = _userSet
                .Include(u => u.AlumniProfile)
                .FirstOrDefault(u => u.Id == currentUserId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            return Ok(Mapper.Map<UserModel>(currentUser));
        }
        
        //POST me/request-bill
        [HttpPost("request-bill")]
        public IActionResult RequestBill()
        {
            throw new NotImplementedException();
        }

        //GET me/donations
        [HttpGet("dontations")]
        public IActionResult GetUserDonations()
        {
            var currentUserId = User.GetId();
            var (donations, income) = _userAccountingReadModel.GetCapitalizationInfo(currentUserId);
            
            return Ok(new
            {
                Donations = donations,
                Income = income
            });
        }
    }
}