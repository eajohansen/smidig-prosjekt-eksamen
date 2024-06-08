using System.Security.Claims;
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

        // GET: api/user/fetchAll
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

        // GET api/user/fetch/id
        [Authorize]
        [HttpGet("fetch/{id}")]
        public async Task<IActionResult> FetchUserById(int id) {

            try {
                User? result = await _userService.FetchUserById(id);
                if (result == null) {
                    return NoContent();
                }
                
                string? userName = User.FindFirstValue(ClaimTypes.Name);
                bool isLoggedInUser = result.Email.Equals(userName);

                if (!isLoggedInUser) {
                    return Unauthorized();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }

        // GET api/user/fetch/email/gunnar@gunnarsen.no
        [Authorize]
        [HttpGet("fetch/email/{email}")]
        public async Task<IActionResult> FetchUserByEmail(string? email) {
            if (email == null) {
                return BadRequest("Email is null");
            }
            
            string? userName = User.FindFirstValue(ClaimTypes.Name);
            bool isLoggedInUser = email.Equals(userName);

            if (!isLoggedInUser) {
                return Unauthorized();
            }
            
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
        public async Task<IActionResult> AddUser(User? user) {
            if (user == null) {
                return BadRequest("User is null");
            }
            try {
                object isAdded = await _userService.AddUserToDatabase(user);
                if (isAdded.Equals(user.Email + " has been added to the database.")) {
                    return Ok(user.Email + " has been added to the database.");
                    
                }
                return BadRequest(isAdded);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        // POST api/user/organizer/create/5
        [Authorize]
        [HttpPost("organizer/create/{loggedInUserId}")]
        public async Task<IActionResult> AddOrganizer([FromRoute] int loggedInUserId, [FromBody] User? user) {
            int organizationId = 1;
            if (user == null) {
                return BadRequest("User is null");
            }
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
        [Authorize]
        [HttpPost("follower/create/{organizationId}")]
        public async Task<IActionResult> AddFollower([FromBody] User? user, [FromRoute] int organizationId) {
            if (user == null) {
                return BadRequest("User is null");
            }
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
        [Authorize]
        [HttpPost("event/add/{eventId}")]
        public async Task<IActionResult> AddUserEvent([FromBody] User? user, [FromRoute] int eventId) {
            if (user == null) {
                return BadRequest("User is null");
            }
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

        // PUT api/user/update
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(User? user) {
            if (user == null) {
                return BadRequest("User is null");
            }

            string? userName = User.FindFirstValue(ClaimTypes.Name);
            bool isLoggedInUser = user.Email.Equals(userName);

            if (!isLoggedInUser) {
                return Unauthorized();
            }
            
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
        [Authorize]
        [HttpPut("admin/update/{id}")]
        public async Task<IActionResult> MakeUserAdmin([FromBody] User? adminUser, [FromRoute] int id) {
            if (adminUser == null) {
                return BadRequest("AdminUser is null");
            }
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

        #endregion

        #region DELETE

        // DELETE api/User/delete
        [Authorize]
        [HttpDelete("delete")]
        public IActionResult Delete(User? user) {
            if (user == null) {
                return BadRequest("User is null");
            }
            
            string? userName = User.FindFirstValue(ClaimTypes.Name);
            bool isLoggedInUser = user.Email.Equals(userName);

            if (!isLoggedInUser) {
                return Unauthorized();
            }

            try {
                bool isDeleted = _userService.DeleteUser(user).Result;
                if (!isDeleted) {
                    // Could not delete user, because request is bad
                    BadRequest();
                }

                return Ok();
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        #endregion
    }
}