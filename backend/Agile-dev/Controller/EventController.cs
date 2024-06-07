using agile_dev.Models;
using agile_dev.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        
        // GET: api/event/fetchAll/attending/1
        [HttpGet("fetchAll/attending/{userID}")]
        public async Task<ActionResult> FetchAllEventsByAttending([FromRoute] int userID) {
            try {
                ICollection<Event> result = await _eventService.FetchAllEventsByAttending(userID);
                if (result.Count == 0) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/event/fetchAll/not/attending/1
        [HttpGet("fetchAll/not/attending/{userID}")]
        public async Task<ActionResult> FetchAllEventsByNotAttending([FromRoute] int userID) {
            try {
                ICollection<Event> result = await _eventService.FetchAllEventsByNotAttending(userID);
                if (result.Count == 0) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }
        
        // GET: api/event/fetchAll/organization/1
        [HttpGet("fetchAll/organization/{organizationID}")]
        public async Task<ActionResult> FetchAllEventsByOrganization([FromRoute] int organizationID) {
            try {
                ICollection<Event> result = await _eventService.FetchAllEventsByOrganization(organizationID);
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
        [HttpGet("fetchAll/not/organization/{organizationID}")]
        public async Task<ActionResult> FetchAllEventsByOtherOrganizations([FromRoute] int organizationID) {
            try {
                ICollection<Event> result = await _eventService.FetchAllEventsByOtherOrganizations(organizationID);
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
        
        // GET: api/event/customfield/fetchAll
        [HttpGet("customfield/fetchAll/{eventId}")]
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
        
        // POST api/event/create/5/6
        [HttpPost("create/{userId}/{organizationId}")]
        public async Task<IActionResult> AddEvent([FromRoute] int userId, [FromBody] Event eEvent, [FromRoute] int organizationId) {
            try {
                bool isAdded = await _eventService.AddEvent(userId, eEvent, organizationId);
                if (!isAdded) {
                    // Could not create event, because request is bad
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while adding event to database.", exception);
            }
        }
        
        #endregion

        #region PUT

        // PUT api/event/update/5/6
        [HttpPut("update/{userId}/{organizationId}")]
        public async Task<IActionResult> UpdateEvent([FromRoute] int userId, [FromRoute] int organizationId, [FromBody] Event eEvent) {
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
        
        // PUT api/event/place/update/5/6/3
        [HttpPut("update/place/{userId}/{organizationId}/{eventId}")]
        public async Task<IActionResult> UpdatePlace([FromRoute] int userId, [FromRoute] int organizationId, [FromRoute] int eventId, [FromBody] Place place) {
            try {
                bool isAdded = await _eventService.UpdatePlace(userId, organizationId, eventId, place);
                if (!isAdded) {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while updating eventDateTime.", exception);
            }
        }
        
        // PUT api/event/contactperson/update/5/6/3
        [HttpPut("update/contactperson/{userId}/{organizationId}/{eventId}")]
        public async Task<IActionResult> UpdateContactPerson([FromRoute] int userId, [FromRoute] int organizationId, [FromRoute] int eventId, [FromBody] ContactPerson contactPerson) {
            try {
                bool isAdded = await _eventService.UpdateContactPerson(userId, organizationId, eventId, contactPerson);
                if (!isAdded) {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while updating eventDateTime.", exception);
            }
        }
        
        // PUT api/event/customfield/update/5/6/3
        [HttpPut("update/customfield/{userId}/{organizationId}/{eventId}")]
        public async Task<IActionResult> UpdateCustomField([FromRoute] int userId, [FromRoute] int organizationId, [FromBody] List<CustomField> customFields) {
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
        [HttpDelete("delete/{userId}/{organizationId}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int userId, [FromBody] Event eEvent, [FromRoute] int organizationId) {
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
