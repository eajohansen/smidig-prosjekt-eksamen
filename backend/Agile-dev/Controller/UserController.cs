using agile_dev.Models;
using agile_dev.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace agile_dev.Controller {
    
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly UserService _userService;
        public UserController(UserService userService) {
            _userService = userService;
        }
        // GET: api/User/fetchAll
        [HttpGet("fetchAll")]
        public async Task<ICollection<User>> FetchAllUsers() {
            ICollection<User> result = await _userService.FetchAllUsers();
            return result;
        }

        // GET api/User/fetch/5
        [HttpGet("fetch/{id}")]
        public string Get(int id) {
            return "value";
        }

        // POST api/User/create
        [HttpPost("create")]
        public async Task<IActionResult> AddUser(User user) {
            var result = await _userService.AddUserToDatabase(user);
            return result;
        }

        // PUT api/User/update/5
        [HttpPut("update/{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/User/delete/5
        [HttpDelete("delete/{id}")]
        public void Delete(int id) {
        }
    }
}
