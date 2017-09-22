using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Authentification;
using MIPTCore.Models;
using UserManagment;

namespace MIPTCore.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly IUserManager _userManager;

        public AuthentificationController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        // POST login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var intentedUser = await _userManager.GetUserByEmailAsync(credentials.Email);
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

            await _userManager.UpdateUserAsync(intentedUser);
            
            var myclaims = new List<Claim>(new Claim[] 
            { 
                new Claim(ClaimTypes.NameIdentifier, intentedUser.Id.ToString()),
                new Claim(ClaimTypes.Role, intentedUser.Role.ToString())
            });

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(myclaims, "MIPTCoreCookieAuthenticationScheme"));

            await HttpContext.SignInAsync("MIPTCoreCookieAuthenticationScheme", claimsPrincipal);

            return Ok(Mapper.Map<UserModel>(intentedUser));
        }

        // POST logout
        [HttpPost("logout")]
        [Authorize("User")]
        public async Task<IActionResult> Logout()
        {
            var currentUserId = User.GetId();

            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            
            currentUser.AuthentificatedAt = null;

            await _userManager.UpdateUserAsync(currentUser);
            
            await HttpContext.SignOutAsync("MIPTCoreCookieAuthenticationScheme");
            
            return Ok();
        }

        // POST me
        [HttpGet("me")]
        [Authorize("User")]
        public async Task<IActionResult> Current()
        {
            var currentUserId = User.GetId();
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            return Ok(Mapper.Map<UserModel>(currentUser));
        }
    }
}