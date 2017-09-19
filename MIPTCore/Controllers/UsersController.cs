using System.Linq;
using System.Threading.Tasks;
using Common.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Authentification;
using MIPTCore.Extensions;
using MIPTCore.Models;
using MIPTCore.Models.Mapper;
using UserManagment;

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

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var all = await _userManager.GetAllUsersAsync();
            return Ok(all.Select(UserMapper.UserToUserModel));
        }

        // GET api/values/5
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
            
            return Ok(UserMapper.UserToUserModel(userToReturn));
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRegistrationModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = UserMapper.UserRegistrationModelToUser(userModel);

            var userId = await _userManager.CreateUserAsync(userToCreate);

            return Ok(userId);
        }

        // PUT api/values/5
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
            userToUpdate.Email = userModel.EmailAddress;
            userToUpdate.IsMiptAlumni = userModel.IsMiptAlumni;
            userToUpdate.AlumniProfile = UserMapper.AlumniProfileModelToAlumniProfile(userModel.AlumniProfile);

            await _userManager.UpdateUserAsync(userToUpdate);
            
            //!!!
            var updatedUser = await _userManager.GetUserByIdAsync(id);

            return Ok(UserMapper.UserToUserModel(updatedUser));
        }

        // DELETE api/values/5
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