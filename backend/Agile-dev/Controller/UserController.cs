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

        #region GET

        // GET: api/User/fetchAll
        [HttpGet("fetchAll")]
        public async Task<ActionResult> FetchAllUsers() {
            try {
                ICollection<User> result = await _userService.FetchAllUsers();
                if (result.Count == 0) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }

        // GET api/User/fetch/5
        [HttpGet("fetch/{id}")]
        public async Task<IActionResult> FetchUserById(int id) {
            try {
                User? result = await _userService.FetchUserById(id);
                if (result == null) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }

        #endregion

        #region POST

        // POST api/User/create
        [HttpPost("create")]
        public async Task<IActionResult> AddUser(User user) {
            var result = await _userService.AddUserToDatabase(user);
            return result;
        }

        #endregion

        #region PUT

        // PUT api/User/update/5
        [HttpPut("update/{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        #endregion

        #region DELETE

        // DELETE api/User/delete/5
        [HttpDelete("delete/{id}")]
        public void Delete(int id) {
        }

        #endregion
    }
}