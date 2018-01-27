using System;
using Common.Entities;
using Journalist.Extensions;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Extensions;
using UserManagment.Application;

namespace MIPTCore.Controllers
{
    [Route("Users")]
    public class UserServiceController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IUserMailerService _userMailerService;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;

        public UserServiceController(IUserMailerService userMailerService, IUserService userService, ITicketService ticketService, IUserManager userManager)
        {
            _userMailerService = userMailerService;
            _userService = userService;
            _ticketService = ticketService;
            _userManager = userManager;
        }

        // POST users/confirm/5
        [HttpPost("confirm/{userId}")]
        public IActionResult BeginConfirmingEmail(int userId)
        {
            this.CheckIdViaModel(userId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userToRecovery = _userManager.GetUserById(userId);

            if (userToRecovery == null)
            {
                return NotFound("User with this ID is not exists");
            }
            
            _userMailerService.BeginEmailConfirmation(userId).Start();
            return Ok();
        }
        
        // POST users/recovery/
        [HttpPost("recovery/{emailToRecover}")]
        public IActionResult BeginRecoveringPassword(string emailToRecover)
        {
            if (emailToRecover.IsEmpty())
                return BadRequest("Email To recover can't be empty");
            
            var userToRecovery = _userManager.GetUserByEmail(emailToRecover);

            if (userToRecovery == null)
            {
                return NotFound("User with this ID is not exists");
            }
            
            _userMailerService.BeginPasswordRecovery(userToRecovery.Id).Start();
            return Ok();
        }
        
        // POST users/initialVerification/token/mYSec0reT0k3n
        [HttpPost("initialVerification/token/{token}")]
        public IActionResult SetPasswordAndConfirmEmailEmail(string token, [FromBody]string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailToconfirmAndSetPassword = _ticketService.GetUserEmailByCombinatedTicket(token);

            if (emailToconfirmAndSetPassword.IsNullOrEmpty())
            {
                return NotFound("Initial user settings token not found");
            }

            var userToconfirmAndSetPassword = _userManager.GetUserByEmail(emailToconfirmAndSetPassword);
            
            _userService.ConfirmEmail(emailToconfirmAndSetPassword);
            _userService.ChangePassword(userToconfirmAndSetPassword.Id, new Password(newPassword));
            return Ok();
        }

        // POST users/confirm/token/mYSec0reT0k3n
        [HttpPost("confirm/token/{token}")]
        public IActionResult ConfirmEmail(string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailToconfirm = _ticketService.GetUserEmailByEmailConfirmationToken(token);

            if (emailToconfirm.IsNullOrEmpty())
            {
                return NotFound("Email confirmation token not found");
            }
            
            _userService.ConfirmEmail(emailToconfirm);
            return Ok();
        }
        
        // POST users/confirm/token/mYSec0reT0k3n
        [HttpPost("recovery/token/{token}")]
        public IActionResult ChangePasswordByrecoveyToken(string token, [FromBody]string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailToRecoverPassword = _ticketService.GetUserEmailByPasswordRecoveyToken(token);
            
            if (emailToRecoverPassword.IsNullOrEmpty())
            {
                return NotFound("Token already used or not found");
            }

            var userToChangePassword = _userManager.GetUserByEmail(emailToRecoverPassword);

            if (userToChangePassword == null)
            {
                return NotFound("Email extracted, but user not found. Token broken");
            }

            Password password;
            try
            {
                password = new Password(newPassword);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            
            _userService.ChangePassword(userToChangePassword.Id, password);
            return Ok();
        }
    }
}