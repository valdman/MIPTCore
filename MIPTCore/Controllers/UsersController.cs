using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Journalist.Extensions;
using Journalist.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Authentification;
using MIPTCore.Extensions;
using MIPTCore.Models;
using UserManagment;
using Mapper = AutoMapper.Mapper;

namespace MIPTCore.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IUserMailerService _userMailerService;

        public UsersController(IUserManager userManager, IUserMailerService userMailerService)
        {
            _userManager = userManager;
            _userMailerService = userMailerService;
        }

        // GET users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var all = await _userManager.GetAllUsersAsync();
            
            return Ok(all.Select(Mapper.Map<UserModel>));
        }

        // GET users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userToReturn = await _userManager.GetUserByIdAsync(id);

            if (userToReturn == null)
            {
                return NotFound("User with this ID is not exists");
            }
            
            return Ok(Mapper.Map<UserModel>(userToReturn));
        }

        // POST users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRegistrationModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = Mapper.Map<User>(userModel);

            var userId = await _userManager.CreateUserAsync(userToCreate);

            return Ok(userId);
        }

        // PUT users/5
        [HttpPut("{id}")]
        [Authorize("User")]
        public async Task<IActionResult> Put(int id, [FromBody] UserUpdateModel userModel)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToUpdate = await _userManager.GetUserByIdAsync(id);

            if (userToUpdate == null)
            {
                return NotFound("User not found");
            }
            
            if (userToUpdate.Id != User.GetId() && User.IsInRole("User"))
            {
                return Unauthorized();
            }

            userToUpdate.FirstName = userModel.FirstName;
            userToUpdate.LastName = userModel.LastName;
            userToUpdate.Email = userModel.Email;
            userToUpdate.IsMiptAlumni = userModel.IsMiptAlumni;
            userToUpdate.AlumniProfile = Mapper.Map<AlumniProfile>(userModel.AlumniProfile);
            
            await _userManager.UpdateUserAsync(userToUpdate);
            
            //!!!
            var updatedUser = await _userManager.GetUserByIdAsync(id);

            return Ok(Mapper.Map<UserModel>(updatedUser));
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

        // POST users/confirm/token/mYSec0reT0k3n
        [HttpPost("confirm/token/{token}")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailToconfirm = await _userMailerService.GetUserEmailByEmailConfirmationToken(token);

            if (emailToconfirm.IsNullOrEmpty())
            {
                return NotFound("Email confirmation token not found");
            }
            
            await _userMailerService.ConfirmEmail(emailToconfirm);
            return Ok();
        }
        
        // POST users/recovery/5
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
        
        // POST users/confirm/token/mYSec0reT0k3n
        [HttpPost("recovery/token/{token}")]
        public async Task<IActionResult> ChangePasswordByrecoveyToken(string token, [FromBody]string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailToRecoverPassword = await _userMailerService.GetUserEmailByPasswordRecoveyToken(token);
            var userToChangePassword = await _userManager.GetUserByEmailAsync(emailToRecoverPassword);

            if (emailToRecoverPassword.IsNullOrEmpty())
            {
                return NotFound();
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
            
            await _userMailerService.ChangePassword(userToChangePassword.Id, password);
            return Ok();
        }
        
        // DELETE users/5
        [HttpDelete("{id}")]
        [Authorize("User")]
        public async Task<IActionResult> Delete(int id)
        {
            this.CheckIdViaModel(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userToDelete = await _userManager.GetUserByIdAsync(id);
            
            if (userToDelete == null)
            {
                return NotFound("User not found");
            }

            if (userToDelete.Id != User.GetId() && User.IsInRole("User"))
            {
                return Unauthorized();
            }

            await _userManager.DeleteUserAsync(id);

            return Ok();
        }
    }
}