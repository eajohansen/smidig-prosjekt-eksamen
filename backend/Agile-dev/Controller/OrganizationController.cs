using agile_dev.Models;
using agile_dev.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agile_dev.Controller {
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase {
        private readonly OrganizationService _organizationService;

        #region GET
        
        // GET: api/organization/fetchAll
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
        
        // POST api/organization/create/5
        [HttpPost("create/{userId}")]
        public async Task<IActionResult> AddOrganization([FromRoute] int userId , [FromBody] Organization organization) {
            try {
                bool isAdded = await _organizationService.AddOrganization(userId, organization);
                if (!isAdded) {
                    // Could not create organization, because request is bad
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

        // PUT api/organization/update/5
        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateOrganization([FromRoute] int userId, [FromBody] Organization organization) {
            try {
                bool isAdded = await _organizationService.UpdateOrganization(userId, organization);
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

        // DELETE api/organization/delete/5
        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteOrganization([FromRoute] int userId, [FromBody] Organization? organization) {
            if (organization == null) {
                return BadRequest("User is null");
            }

            try {
                bool isDeleted = _organizationService.DeleteOrganization(userId, organization).Result;
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
