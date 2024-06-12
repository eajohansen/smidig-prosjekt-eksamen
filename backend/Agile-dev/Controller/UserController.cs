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
                HandleReturn<List<UserFrontendDto>> result = await _userService.FetchAllUsers();
                if (!result.IsSuccess) {
                    return NotFound(result.ErrorMessage);
                }

                return Ok(result.Value);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }

        // GET api/user/fetch/id
        [Authorize]
        [HttpGet("fetch/id/{id}")]
        public async Task<IActionResult> FetchUserById(string id) {
            try {
                HandleReturn<UserFrontendDto> result = await _userService.FetchUserById(id);
                if (!result.IsSuccess) {
                    return NotFound(result.ErrorMessage);
                }

                if (result.Value!.Email == null) {
                    return BadRequest("Email is null");
                }
                
                string? userName = User.FindFirstValue(ClaimTypes.Name);
                
                bool isLoggedInUser = result.Value!.Email.Equals(userName);

                if (!isLoggedInUser) {
                    return Unauthorized("You are not authorized to view this user, you can only view your own user");
                }

                return Ok(result.Value);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/user/fetch/email
        [Authorize]
        [HttpGet("fetch/email")]
        public ActionResult FetchUserEmail() {
            try {
                string userEmail = User.FindFirstValue(ClaimTypes.Email)!;
                return Ok(userEmail);
            }
            catch (Exception e) {
                return StatusCode(500 , "Internal server error: " + e.Message);
            }
        }

        // GET: api/user/checkAdminPrivileges
        [Authorize]
        [HttpGet("checkAdminPrivileges")]
        public async Task<IActionResult> CheckIfUserIsAdminOrOrganizer() {
            try {
                string userEmail = User.FindFirstValue(ClaimTypes.Email)!;
                HandleReturn<UserFrontendDto> user = await _userService.FetchUserByEmail(userEmail);
                
                if (!user.IsSuccess) {
                    return NotFound(user.ErrorMessage);
                }
                
                object feedback = new {
                    Admin = _userService.CheckIfUserIsAdmin(userEmail).Result.Value,
                    Organizer = user.Value!.OrganizerOrganization!.Count != 0
                };
                
                return Ok(feedback);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }

        #endregion

        #region POST
        
        // POST api/user/add/organizer/1
        [Authorize(Roles = "Admin")]
        [HttpPost("add/organizer/{organizationId}")]
        public async Task<IActionResult> AddOrganizer([FromRoute] int organizationId, User? userToAdd) {

            if (organizationId < 1) {
                return BadRequest("No organizationId provided");
            }
            
            if (userToAdd!.Email == null) {
                return BadRequest("No email provided in request");
            }

            try {
                IdentityResult makeUserOrganizer = await _userService.AddUserAsOrganizer(organizationId, userToAdd.Email);

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

        // POST api/user/add/follower/1
        [Authorize]
        [HttpPost("add/follower/{organizationId}")]
        public async Task<IActionResult> AddFollower([FromRoute] int organizationId) {
            if (organizationId < 1) {
                return BadRequest("No organizationId provided");
            }
            try {
                string userEmail = User.FindFirstValue(ClaimTypes.Email)!;
                
                HandleReturn<bool> isAdded = await _userService.AddUserAsFollower(userEmail, organizationId);
                if (!isAdded.IsSuccess) {
                    // Could not add user as follower, because request is bad
                    return BadRequest(isAdded.ErrorMessage);
                }

                return Ok();
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        // POST api/user/add/event/3
        [Authorize]
        [HttpPost("add/event/{eventId}")]
        public async Task<IActionResult> AddUserEvent([FromRoute] int eventId) {
            if (eventId < 1) {
                return BadRequest("No eventId provided");
            }
            try {
                string userEmail = User.FindFirstValue(ClaimTypes.Email)!;
                HandleReturn<bool> isAdded = await _userService.AddUserToEvent(userEmail, eventId);
                if (!isAdded.IsSuccess) {
                    // Could not add user to event, because request is bad
                    return BadRequest(isAdded.ErrorMessage);
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
        public async Task<IActionResult> UpdateUserInfo(User updatedUserInfo){
            if (updatedUserInfo.Email == null){
                return BadRequest("No email provided in request");
            }

            if (updatedUserInfo.Email != User.FindFirstValue(ClaimTypes.Email)) {
                return Unauthorized("You are not authorized to update this user");
            }
            try {
                IdentityResult updateUser = await _userService.UpdateUserAsync(updatedUserInfo);

                return Ok(updateUser);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        // PUT api/user/add/admin
        [Authorize (Roles="Admin")]
        [HttpPut("add/admin")]
        public async Task<IActionResult> MakeUserAdmin(User? newAdminUser) {
            if (newAdminUser == null) {
                return BadRequest("AdminUser is null");
            }
            if (newAdminUser.Email == null){
                return BadRequest("No email provided in request");
            }
            try {
                IdentityResult makeUserAdmin = await _userService.MakeUserAdmin(newAdminUser.Email);

                if (!makeUserAdmin.Succeeded) {
                    return BadRequest(makeUserAdmin.Errors);
                }
                
                return Ok(makeUserAdmin);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        #endregion
    
        #region DELETE

        // DELETE api/user/delete
        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteUser() {
            string userEmail = User.FindFirstValue(ClaimTypes.Email)!;
            
            try {
                HandleReturn<bool> isDeleted = _userService.DeleteUser(userEmail).Result;
                if (!isDeleted.IsSuccess) {
                    // Could not delete user, because request is bad
                    BadRequest(isDeleted.ErrorMessage);
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