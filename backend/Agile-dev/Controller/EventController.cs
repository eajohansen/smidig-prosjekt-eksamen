using System.Security.Claims;
using agile_dev.Dto;
using agile_dev.Models;
using agile_dev.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agile_dev.Controller {
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase {
        private readonly EventService _eventService;
        

        public EventController(EventService eventService) {
            _eventService = eventService;
        }

        #region GET
        
        // GET: api/event/fetchAll
        [HttpGet("fetchAll")]
        public async Task<ActionResult> FetchAllEvents() {
            try {
                ICollection<Event> result = await _eventService.FetchAllEvents();
                if (result.Count == 0) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/event/fetchAll/attending
        [Authorize]
        [HttpGet("fetchAll/attending")]
        public async Task<ActionResult> FetchAllEventsByAttending() {
            try {
                string? userName = User.FindFirstValue(ClaimTypes.Name);
                ICollection<Event>? result = await _eventService.FetchAllEventsByAttending(userName!);
                if (result != null) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/event/fetchAll/not/attending/1
        [Authorize]
        [HttpGet("fetchAll/not/attending/{userId}")]
        public async Task<ActionResult> FetchAllEventsByNotAttending([FromRoute] string userId) {
            try {
                string? userName = User.FindFirstValue(ClaimTypes.Name);
                ICollection<Event>? result = await _eventService.FetchAllEventsByNotAttending(userName!);
                if (result != null) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/event/fetchAll/organization/1
        [HttpGet("fetchAll/organization/{organizationId}")]
        public async Task<ActionResult> FetchAllEventsByOrganization([FromRoute] int organizationId) {
            try {
                ICollection<Event> result = await _eventService.FetchAllEventsByOrganization(organizationId);
                if (result.Count == 0) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/event/fetchAll/not/organization/1
        [HttpGet("fetchAll/not/organization/{organizationId}")]
        public async Task<ActionResult> FetchAllEventsByOtherOrganizations([FromRoute] int organizationId) {
            try {
                ICollection<Event> result = await _eventService.FetchAllEventsByOtherOrganizations(organizationId);
                if (result.Count == 0) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }

        // GET api/event/fetch/id/5
        [HttpGet("fetch/id/{id}")]
        public async Task<IActionResult> FetchEventById(int id) {
            try {
                Event? result = await _eventService.FetchEventById(id);
                if (result == null) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/event/customField/fetchAll
        [HttpGet("customField/fetchAll/{eventId}")]
        public async Task<ActionResult> FetchAllCustomFields(int eventId) {
            try {
                ICollection<CustomField?> result = await _eventService.FetchAllCustomFields(eventId);

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        #endregion

        #region POST
        
        // POST api/event/create
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> AddEvent([FromBody] EventDto? frontendEvent) {
            try {
                if (frontendEvent == null) {
                    return BadRequest("Event is required");
                }
                
                string? userName = User.FindFirstValue(ClaimTypes.Name);
                if(userName == null) {
                    return Unauthorized("Invalid user");
                }
                
                object newEvent = await _eventService.AddEvent(userName, frontendEvent);
                if (newEvent is not Event) {
                    // Could not create event, because request is bad
                    return BadRequest(newEvent);
                }

                return Ok(newEvent);
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while adding event to database.", exception);
            }
        }
        
        #endregion

        #region PUT

        // PUT api/event/update/5/6
        [Authorize]
        [HttpPut("update/{userId}/{organizationId}")]
        public async Task<IActionResult> UpdateEvent([FromRoute] string userId, [FromRoute] int organizationId, [FromBody] Event eEvent) {
            try {
                bool isAdded = await _eventService.UpdateEvent(userId, organizationId, eEvent);
                if (!isAdded) {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while updating event.", exception);
            }
        }
        
        // PUT api/event/customField/update/5/6/3
        [Authorize]
        [HttpPut("update/customField/{userId}/{organizationId}/{eventId}")]
        public async Task<IActionResult> UpdateCustomField([FromRoute] string userId, [FromRoute] int organizationId, [FromBody] List<CustomField> customFields) {
            try {
                bool isAdded = await _eventService.UpdateCustomField(userId, organizationId, customFields);
                if (!isAdded) {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while updating eventDateTime.", exception);
            }
        }
        
        #endregion

        #region DELETE

        // DELETE api/event/delete/5/6
        [Authorize]
        [HttpDelete("delete/{userId}/{organizationId}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] string userId, [FromBody] Event eEvent, [FromRoute] int organizationId) {
            try {
                bool isDeleted = await _eventService.DeleteEvent(userId, eEvent, organizationId);
                if (!isDeleted) {
                    // Could not delete event, because request is bad
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
