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
        


        // GET: api/User/fetchAll
        [Authorize]
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

        // GET api/user/fetch/id/5
        [HttpGet("fetch/id/{id}")]
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
        
        // GET api/user/fetch/email/gunnar@gunnarsen.no
        [HttpGet("fetch/email/{email}")]
        public async Task<IActionResult> FetchUserByEmail(string email) {
            try {
                User? result = await _userService.FetchUserByEmail(email);
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
                    // Could not create user, because request is bad
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        // POST api/user/organizer/create/5/1
        [HttpPost("organizer/create/{loggedInUserId}/{organizationId}")]
        public async Task<IActionResult> AddOrganizer([FromRoute] int loggedInUserId, [FromBody] User user, [FromRoute] int organizationId) {
            try {
                bool isAdded = await _userService.AddUserAsOrganizer(loggedInUserId, user, organizationId);
                if (!isAdded) {
                    // Could not add user as organizer, because request is bad
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        // POST api/user/follower/create/1
        [HttpPost("follower/create/{organizationId}")]
        public async Task<IActionResult> AddFollower([FromBody] User user, [FromRoute] int organizationId) {
            try {
                bool isAdded = await _userService.AddUserAsFollower(user, organizationId);
                if (!isAdded) {
                    // Could not add user as follower, because request is bad
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        // POST api/user/event/add/3
        [HttpPost("event/add/{eventId}")]
        public async Task<IActionResult> AddUserEvent([FromBody] User user, [FromRoute] int eventId) {
            try {
                bool isAdded = await _userService.AddUserToEvent(user, eventId);
                if (!isAdded) {
                    // Could not add user to event, because request is bad
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        #endregion

        #region PUT

        // PUT api/user/update/5
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(User user) {
            try {
                bool isAdded = await _userService.UpdateUser(user);
                if (!isAdded) {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        // PUT api/user/admin/update/5
        // "AdminUser" in this context is the user that is making the request
        // "id" is the id of the user that is being made admin
        [HttpPut("admin/update/{id}")]
        public async Task<IActionResult> MakeUserAdmin([FromBody] User adminUser, [FromRoute] int id) {
            try {
                bool makeUserAdmin = await _userService.MakeUserAdmin(adminUser, id);
                if (!makeUserAdmin) {
                    return Unauthorized();
                }
                return Ok();
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        // PUT api/user/allergies/update
        [HttpPut("allergies/update")]
        public async Task<IActionResult> UpdateUserAllergies(User user) {
            try {
                bool isAdded = await _userService.UpdateUserAllergiesDb(user);
                if (!isAdded) {
                    return BadRequest();
                }
                return Ok();
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