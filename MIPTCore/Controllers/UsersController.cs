using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Infrastructure;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(all);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _userRepository.GetById(id));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}