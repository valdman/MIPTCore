using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Infrastructure;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using MIPTCore.Models;
using UserManagment;

namespace MIPTCore.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IGenericRepository<User> _userRepository;

        public UsersController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var all = await _userRepository.GetAll();
            return Ok(all.Select(UserMapper.UserToUserModel));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userToReturn = await _userRepository.GetById(id);

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

            var userId = await _userRepository.CreateAsync(userToCreate);

            return Ok(userId);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToUpdate = await _userRepository.GetById(id);

            if (userToUpdate == null)
            {
                return NotFound("User not found");
            }
            
            userToUpdate = UserMapper.UserModelToUser(userModel);
            userToUpdate.Id = id;

            await _userRepository.UpdateAsync(userToUpdate);

            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userToUpdate = await _userRepository.GetById(id);

            if (userToUpdate == null)
            {
                return NotFound("User not found");
            }

            await _userRepository.DeleteAsync(id);

            return Ok();
        }
    }
}