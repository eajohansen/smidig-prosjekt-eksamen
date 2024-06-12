using System.Security.Claims;
using agile_dev.Models;
using agile_dev.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agile_dev.Controller {
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase {
        private readonly OrganizationService _organizationService;

        public OrganizationController(OrganizationService organizationService) {
            _organizationService = organizationService;
        }

        #region GET
        
        // GET: api/organization/fetchAll
        [Authorize(Roles = "Admin")]
        [HttpGet("fetchAll")]
        public async Task<ActionResult> FetchAllOrganizations() {
            try {
                HandleReturn<ICollection<Organization>> result = await _organizationService.FetchAllOrganizations();
                if (!result.IsSuccess) {
                    return NotFound(result.ErrorMessage);
                }

                return Ok(result.Value);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }

        // GET api/organization/fetch/id/5
        [Authorize(Roles = "Admin, Organizer")]
        [HttpGet("fetch/id/{id}")]
        public async Task<IActionResult> FetchOrganizationById(int id) {
            try {
                HandleReturn<Organization> result = await _organizationService.FetchOrganizationById(id);
                if (!result.IsSuccess) {
                    return NotFound(result.ErrorMessage); 
                }

                return Ok(result.Value);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }

        #endregion

        #region POST
        
        // POST api/organization/create
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> AddOrganization([FromBody] Organization organization) {
            try {
                HandleReturn<Organization> newOrganization = await _organizationService.AddOrganization(organization);
                if (!newOrganization.IsSuccess) {
                    // Could not create organization, because request is bad
                    return BadRequest(newOrganization.ErrorMessage);
                }

                return Ok(newOrganization.Value);
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        #endregion

        #region PUT

        // PUT api/organization/update
        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateOrganization([FromBody] Organization organization) {
            try {
                HandleReturn<Organization> updatedOrganizationObject = await _organizationService.UpdateOrganization(organization);
                if (!updatedOrganizationObject.IsSuccess) {
                    return BadRequest(updatedOrganizationObject.ErrorMessage);
                }

                return Ok(updatedOrganizationObject.Value);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        #endregion

        #region DELETE

        // DELETE api/organization/delete
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteOrganization([FromBody] Organization organization) {
            try {
                HandleReturn<bool> isDeleted = await _organizationService.DeleteOrganization(organization);
                if (!isDeleted.IsSuccess) {
                    // Could not delete user, because request is bad
                    BadRequest("Could not delete organization");
                }

                return Ok("Organization was deleted");
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        #endregion
    }
}
