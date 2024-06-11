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
                ICollection<Organization> result = await _organizationService.FetchAllOrganizations();
                if (result.Count == 0) {
                    return NoContent();
                }

                return Ok(result);
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
                Organization? result = await _organizationService.FetchOrganizationById(id);
                if (result == null) { return NoContent(); }

                return Ok(result);
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
                object newOrganization = await _organizationService.AddOrganization(organization);
                if (newOrganization is not Organization) {
                    // Could not create organization, because request is bad
                    return BadRequest(newOrganization);
                }

                return Ok(newOrganization);
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        #endregion

        #region PUT

        // PUT api/organization/update
        [Authorize(Roles = "Admin, Organizer")]
        [HttpPut("update")]
        public async Task<object> UpdateOrganization([FromBody] Organization organization) {
            try {
                object updatedOrganizationObject = await _organizationService.UpdateOrganization(organization);
                if (updatedOrganizationObject is not Organization updatedOrganization) {
                    return BadRequest(updatedOrganizationObject);
                }

                return Ok(updatedOrganization);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }
        
        #endregion

        #region DELETE

        // DELETE api/organization/delete/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteOrganization([FromBody] Organization? organization) {
            if (organization == null) {
                return BadRequest("User is null");
            }

            try {
                bool isDeleted = await _organizationService.DeleteOrganization(organization);
                if (!isDeleted) {
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
