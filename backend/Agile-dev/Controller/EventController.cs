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
        [Authorize]
        [HttpGet("fetchAll")]
        public async Task<ActionResult> FetchAllEvents() {
            try {
                HandleReturn<ICollection<EventDtoBackend>> result = await _eventService.FetchAllEvents();
                if (!result.IsSuccess) {
                    return NotFound(result.ErrorMessage);
                }

                return Ok(result.Value);
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
                string userName = User.FindFirstValue(ClaimTypes.Name)!;
                HandleReturn<ICollection<EventDtoBackend>> result = await _eventService.FetchAllEventsByAttending(userName);
                if (!result.IsSuccess) {
                    return NotFound(result.ErrorMessage);
                }

                return Ok(result.Value);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/event/fetchAll/not/attending
        [Authorize]
        [HttpGet("fetchAll/not/attending")]
        public async Task<ActionResult> FetchAllEventsByNotAttending() {
            try {
                string? userName = User.FindFirstValue(ClaimTypes.Name);
                HandleReturn<ICollection<EventDtoBackend>> result = await _eventService.FetchAllEventsByNotAttending(userName!);
                if (!result.IsSuccess) {
                    return NotFound(result.ErrorMessage);
                }

                return Ok(result.Value);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/event/fetchAll/organization/1
        [Authorize]
        [HttpGet("fetchAll/organization/{organizationId}")]
        public async Task<ActionResult> FetchAllEventsByOrganization([FromRoute] int organizationId) {
            try {
                HandleReturn<ICollection<EventDtoBackend>> result = await _eventService.FetchAllEventsByOrganization(organizationId);
                if (!result.IsSuccess) {
                    return NotFound(result.ErrorMessage);
                }

                return Ok(result.Value);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/event/fetchAll/not/organization/1
        // [Authorize]
        // [HttpGet("fetchAll/not/organization/{organizationId}")]
        // public async Task<ActionResult> FetchAllEventsByOtherOrganizations([FromRoute] int organizationId) {
        //     try {
        //         ICollection<Event> result = await _eventService.FetchAllEventsByOtherOrganizations(organizationId);
        //         if (result.Count == 0) {
        //             return NoContent();
        //         }
        //
        //         return Ok(result);
        //     }
        //     catch (Exception exception) {
        //         return StatusCode(500, "Internal server Error: " + exception.Message);
        //     }
        // }

        // GET api/event/id/fetch/5
        [HttpGet("fetch/id/{id}")]
        public async Task<IActionResult> FetchEventById(int id) {
            try {
                HandleReturn<EventDtoBackend> result = await _eventService.FetchEventById(id);
                if (!result.IsSuccess) {
                    return NotFound(result.ErrorMessage);
                }

                return Ok(result.Value);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        /*
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
        }*/
        
        #endregion

        #region POST
        
        // POST api/event/create
        [Authorize(Roles = "Admin, Organizer")]
        [HttpPost("create")]
        public async Task<IActionResult> AddEvent([FromBody] EventDtoFrontend frontendEvent) {
            try {
                HandleReturn<Event> newEvent = await _eventService.AddEvent(frontendEvent);
                if (!newEvent.IsSuccess) {
                    // Could not create event, because request is bad
                    return BadRequest(newEvent.ErrorMessage);
                }

                return Ok(newEvent.Value);
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while adding event to database.", exception);
            }
        }
        
        #endregion

        #region PUT

        // PUT api/event/update
        [Authorize(Roles = "Admin, Organizer")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateEvent([FromBody] Event eEvent) {
            try {
                HandleReturn<bool> isAdded = await _eventService.UpdateEvent(eEvent);
                if (!isAdded.IsSuccess) {
                    return BadRequest(isAdded.ErrorMessage);
                }

                return Ok();
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while updating event.", exception);
            }
        }
        
        #endregion

        #region DELETE

        // DELETE api/event/delete
        [Authorize(Roles = "Admin, Organizer")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteEvent([FromBody] Event eEvent) {
            try {
                HandleReturn<bool> isDeleted = await _eventService.DeleteEvent(eEvent);
                if (!isDeleted.IsSuccess) {
                    // Could not delete event, because request is bad
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
