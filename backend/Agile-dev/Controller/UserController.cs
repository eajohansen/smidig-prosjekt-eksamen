using agile_dev.Models;
using agile_dev.Service;
using Microsoft.AspNetCore.Authorization;
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
        
        /*
            Using Async methods to avoid blocking the main thread.
            This is important because the main thread is responsible for handling incoming requests.
            
            Exceptions are thrown when an error occurs.
            This is important bacause we have to send the error code to the client.
            
            Task<> is used because we are using async methods.
            
            IActionResult is used because we are returning a response.
         */

        #region GET
        [Authorize]
        [HttpGet("test")]
      
        public async Task<ActionResult> Test() {
            return Ok();
        }

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

        // POST api/user/create
        [HttpPost("create")]
        public async Task<IActionResult> AddUser(User user) {
            try {
                bool isAdded = await _userService.AddUserToDatabase(user);
                if (!isAdded) {
                    // Could create user, because request is bad
                    return BadRequest();
                }
                return Ok(user);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        #endregion

        #region PUT

        // PUT api/User/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(User user) {
            try {
                bool isAdded = await _userService.UpdateUser(user);
                if (!isAdded) {
                    return BadRequest();
                }
                return Ok(user);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        [HttpPut("admin")]
        public async Task<IActionResult> MakeUserAdmin([FromBody] User adminUser, [FromRoute] User user) {
            try {
                bool makeUserAdmin = await _userService.MakeUserAdmin(adminUser, user);
                if (!makeUserAdmin) {
                    return Unauthorized();
                }
                return Ok(user);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        #endregion

        #region DELETE

        // // DELETE api/User/delete/5
        // [HttpDelete("delete/{id}")]
        // public void Delete(int id) {
        // }

        #endregion
    }
}