using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Authentification;
using MIPTCore.Extensions;
using MIPTCore.Models;
using UserManagment;
using UserManagment.Application;
using Mapper = AutoMapper.Mapper;

namespace MIPTCore.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
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