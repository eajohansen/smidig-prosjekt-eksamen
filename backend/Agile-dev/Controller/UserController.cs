using System.Security.Claims;
using agile_dev.Dto;
using agile_dev.Models;
using agile_dev.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            This is important because we have to send the error code to the client.

            Task<> is used because we are using async methods.

            IActionResult is used because we are returning a response.
         */

      
        #region GET

        // GET: api/user/fetchAll
        [Authorize (Roles="Admin, Organizer")]
        [HttpGet("fetchAll")]
        public async Task<ActionResult> FetchAllUsers() {
            try {
                List<UserFrontendDto> result = await _userService.FetchAllUsers();
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
        public async Task<IActionResult> FetchUserById(string id) {

            try {
                object result = await _userService.FetchUserById(id);
                if (result is not UserFrontendDto user) {
                    return NoContent();
                }
                
                string? userName = User.FindFirstValue(ClaimTypes.Name);
                if (user.Email == null)
                {
                    return BadRequest("Email is null");
                }
                bool isLoggedInUser = user.Email.Equals(userName);

                if (!isLoggedInUser) {
                    return Unauthorized();
                }

                return Ok(user);
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
                UserFrontendDto? result = await _userService.FetchUserByEmail(email);
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

        // POST api/user/follower/create/1
        [Authorize]
        [HttpPost("follower/create/{organizationId}")]
        public async Task<IActionResult> AddFollower([FromRoute] int organizationId)
        {
            try {
                IdentityResult isAdded = await _userService.AddUserAsFollower(organizationId);
                if (!isAdded.Succeeded) {
                    // Could not add user as follower, because request is bad
                    return BadRequest(isAdded);
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
        public async Task<IActionResult> AddUserEvent([FromRoute] int eventId) {
            if (eventId < 1) {
                return BadRequest("No eventId provided");
            }
            try {
                string userEmail = User.FindFirstValue(ClaimTypes.Email)!;
                IdentityResult isAdded = await _userService.AddUserToEvent(userEmail, eventId);
                if (!isAdded.Succeeded) {
                    // Could not add user to event, because request is bad
                    return BadRequest(isAdded);
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
        public async Task<IActionResult> UpdateUserInfo(User? updatedUserInfo){
            if (updatedUserInfo == null) {
                return BadRequest("User object is null");
            }
            if (updatedUserInfo.Email == null){
                return BadRequest("No email provided in request");
            }

            if (updatedUserInfo.Email != User.FindFirstValue(ClaimTypes.Email)) {
                return Unauthorized("You are not authorized to update this user");
            }
            try {
                IdentityResult updateUser = await _userService.UpdateUserAsync(updatedUserInfo);
                
                //if (updateUser is not Models.User) {
                //    return Unauthorized(updateUser);
                //}

                return Ok(updateUser);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        // PUT api/user/admin/add
        [Authorize (Roles="Admin")]
        [HttpPut("admin/add")]
        public async Task<IActionResult> MakeUserAdmin(User? newAdminUser) {
            if (newAdminUser == null) {
                return BadRequest("AdminUser is null");
            }
            if (newAdminUser.Email == null){
                return BadRequest("No email provided in request");
            }
            try {
                IdentityResult makeUserAdmin = await _userService.MakeUserAdmin(newAdminUser.Email);

                return Ok(makeUserAdmin);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        // PUT api/user/organizer/add/1
        [Authorize(Roles = "Admin")]
        [HttpPut("organizer/add/{orgId}")]
        public async Task<IActionResult> AddOrganizer([FromRoute] int orgId, User? userToAdd)
        {
            if (userToAdd == null)
            {
                return BadRequest("userToAdd is null");
            }

            if (userToAdd.Email == null)
            {
                return BadRequest("No email provided in request");
            }

            try
            {
                IdentityResult makeUserOrganizer = await _userService.AddUserAsOrganizer(orgId, userToAdd.Email);
                //if (makeUserOrganizer is not Models.User)
                //{
                    
                //    return Unauthorized(makeUserOrganizer);
                //}

                if (!makeUserOrganizer.Succeeded) {
                    return BadRequest(makeUserOrganizer);
                }

                return Ok(makeUserOrganizer);
            }
            catch (Exception exception)
            {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        #endregion
    
        #region DELETE

        // DELETE api/user/delete
        [Authorize]
        [HttpDelete("delete")]
        public IActionResult Delete() {
            string userEmail = User.FindFirstValue(ClaimTypes.Email)!;
            
            try {
                bool isDeleted = _userService.DeleteUser(userEmail).Result;
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