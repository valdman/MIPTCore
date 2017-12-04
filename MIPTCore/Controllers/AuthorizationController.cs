using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Authentification;
using MIPTCore.Models;
using UserManagment;
using UserManagment.Application;
using UserManagment.Exceptions;

namespace MIPTCore.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IAuthentificationService _authentificationService;

        public AuthentificationController(IUserManager userManager, IAuthentificationService authentificationService)
        {
            _userManager = userManager;
            _authentificationService = authentificationService;
        }

        // POST login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User intentedUser;
            try
            {
                intentedUser = await _authentificationService.AuthentificateAsync(Mapper.Map<Credentials>(credentials));
            }
            catch (OperationOnUserThatNotExistsException)
            {
                return NotFound("User with that email not found");
            }
            catch (WrongPasswordException)
            {
                return Unauthorized();
            }
            
            var myclaims = new List<Claim>(new[] 
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
            
            await _authentificationService.DeauthentificateAsync(currentUserId);
            
            await HttpContext.SignOutAsync("MIPTCoreCookieAuthenticationScheme");
            
            return Ok();
        }

        // POST me
        [HttpGet("me")]
        [Authorize("User")]
        public IActionResult Current()
        {
            var currentUserId = User.GetId();
            var currentUser = _userManager.GetUserById(currentUserId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            return Ok(Mapper.Map<UserModel>(currentUser));
        }
    }
}