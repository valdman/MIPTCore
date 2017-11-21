using System;
using System.Threading.Tasks;
using Common;
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
        public async Task<IActionResult> BeginConfirmingEmail(int userId)
        {
            this.CheckIdViaModel(userId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userToRecovery = await _userManager.GetUserByIdAsync(userId);

            if (userToRecovery == null)
            {
                return NotFound("User with this ID is not exists");
            }
            
            await _userMailerService.BeginEmailConfirmation(userId);
            return Ok();
        }
        
        // POST users/recovery/
        [HttpPost("recovery/{emailToRecover}")]
        public async Task<IActionResult> BeginRecoveringPassword(string emailToRecover)
        {
            if (emailToRecover.IsEmpty())
                return BadRequest("Email to recover can't be empty");
            
            var userToRecovery = await _userManager.GetUserByEmailAsync(emailToRecover);

            if (userToRecovery == null)
            {
                return NotFound("User with this ID is not exists");
            }
            
            await _userMailerService.BeginPasswordRecovery(userToRecovery.Id);
            return Ok();
        }
        
        // POST users/initialVerification/token/mYSec0reT0k3n
        [HttpPost("initialVerification/token/{token}")]
        public async Task<IActionResult> SetPasswordAndConfirmEmailEmail(string token, [FromBody]string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailToconfirmAndSetPassword = await _ticketService.GetUserEmailByCombinatedTicket(token);

            if (emailToconfirmAndSetPassword.IsNullOrEmpty())
            {
                return NotFound("Initial user settings token not found");
            }

            var userToconfirmAndSetPassword = await _userManager.GetUserByEmailAsync(emailToconfirmAndSetPassword);
            
            await _userService.ConfirmEmail(emailToconfirmAndSetPassword);
            await _userService.ChangePassword(userToconfirmAndSetPassword.Id, new Password(newPassword));
            return Ok();
        }

        // POST users/confirm/token/mYSec0reT0k3n
        [HttpPost("confirm/token/{token}")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailToconfirm = await _ticketService.GetUserEmailByEmailConfirmationToken(token);

            if (emailToconfirm.IsNullOrEmpty())
            {
                return NotFound("Email confirmation token not found");
            }
            
            await _userService.ConfirmEmail(emailToconfirm);
            return Ok();
        }
        
        // POST users/confirm/token/mYSec0reT0k3n
        [HttpPost("recovery/token/{token}")]
        public async Task<IActionResult> ChangePasswordByrecoveyToken(string token, [FromBody]string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailToRecoverPassword = await _ticketService.GetUserEmailByPasswordRecoveyToken(token);
            
            if (emailToRecoverPassword.IsNullOrEmpty())
            {
                return NotFound("Token already used or not found");
            }

            var userToChangePassword = await _userManager.GetUserByEmailAsync(emailToRecoverPassword);

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
            
            await _userService.ChangePassword(userToChangePassword.Id, password);
            return Ok();
        }
    }
}