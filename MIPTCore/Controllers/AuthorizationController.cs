using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common;
using Common.Infrastructure;
using DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Models;
using UserManagment;

namespace MIPTCore.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly IGenericRepository<User> _userRepository;

        public AuthentificationController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("user/login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            var intentedUser = (await _userRepository.FindByAsync(user => user.Email == credentials.Email)).SingleOrDefault();
            if(intentedUser == null)
            {
                return NotFound();
            }

            var intendedHash = new Password(credentials.Password).Hash;

            if(intentedUser.Password.Hash != intendedHash)
            {
                return Unauthorized();
            }
            
            intentedUser.AuthentificatedAt = DateTimeOffset.Now;

            await _userRepository.UpdateAsync(intentedUser);

            var claimsPrincipal = new ClaimsPrincipal(intentedUser);

            await HttpContext.SignInAsync("MIPTCoreCookieAuthenticationScheme", claimsPrincipal);

            return Ok(UserMapper.UserToUserModel(intentedUser));
        }

        [HttpPost("user/logout")]
        [Authorize("User")]
        public async Task<IActionResult> Logout()
        {
            var currentUser = (User)User.Identity;
            currentUser.AuthentificatedAt = null;

            await _userRepository.UpdateAsync(currentUser);
            
            await HttpContext.SignOutAsync("MyCookieAuthenticationScheme");
            return Ok();
        }

        [HttpGet("user/current")]
        [Authorize("User")]
        public IActionResult Current()
        {
            var currentUser = (User)User.Identity;

            if (currentUser == null)
            {
                return Unauthorized();
            }

            return Ok(UserMapper.UserToUserModel(currentUser));
        }
    }
}