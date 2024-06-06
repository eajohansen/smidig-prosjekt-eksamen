using agile_dev.Models;
using agile_dev.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agile_dev.Controller {
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase {
        private readonly EventService _eventService;

        #region GET
        
        // GET: api/event/fetchAll
        [HttpGet("fetchAll")]
        public async Task<ActionResult> FetchAllUsers() {
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

        // GET api/event/fetch/id/5
        [HttpGet("fetch/id/{id}")]
        public async Task<IActionResult> FetchUserById(int id) {
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
                throw new Exception("An error occurred while adding organization to database.", exception);
            }
        }
        
        // POST api/event/datetime/create/5/6
        [HttpPost("datetime/create/{userId}/{organizationId}")]
        public async Task<IActionResult> AddEventDateTime([FromRoute] int userId, [FromBody] EventDateTime eventDateTime,
            [FromRoute] int organizationId) {
            try {
                int isAdded = await _eventService.AddEventDateTime(userId, eventDateTime, organizationId);
                if (isAdded == 0) {
                    return BadRequest();
                }

                return Ok(isAdded);
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while adding eventDateTime to database.", exception);
            }
        }
        
        // POST api/event/place/create/5/6
        [HttpPost("place/create/{userId}/{organizationId}")]
        public async Task<IActionResult> AddPlace([FromRoute] int userId, [FromBody] Place place,
            [FromRoute] int organizationId) {
            try {
                int isAdded = await _eventService.AddPlace(userId, place, organizationId);
                if (isAdded == 0) {
                    return BadRequest();
                }

                return Ok(isAdded);
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while adding eventDateTime to database.", exception);
            }
        }
        
        // POST api/event/contactperson/create/5/6
        [HttpPost("contactperson/create/{userId}/{organizationId}")]
        public async Task<IActionResult> AddContactPerson([FromRoute] int userId, [FromBody] ContactPerson contactPerson,
            [FromRoute] int organizationId) {
            try {
                int isAdded = await _eventService.AddContactPerson(userId, contactPerson, organizationId);
                if (isAdded == 0) {
                    return BadRequest();
                }

                return Ok(isAdded);
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while adding eventDateTime to database.", exception);
            }
        }
        
        // POST api/event/customfield/create/5/6
        [HttpPost("customfield/create/{userId}/{organizationId}")]
        public async Task<IActionResult> AddCustomFields([FromRoute] int userId, [FromBody] List<CustomField> customFields,
            [FromRoute] int organizationId) {
            try {
                List<EventCustomField> isAdded = await _eventService.AddCustomFields(userId, organizationId, customFields);

                return Ok(isAdded);
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while adding eventDateTime to database.", exception);
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
        
        // PUT api/event/datetime/update/5/6/3
        [HttpPut("update/datetime/{userId}/{organizationId}/{eventId}")]
        public async Task<IActionResult> UpdateEventDateTime([FromRoute] int userId, [FromRoute] int organizationId, [FromRoute] int eventId, [FromBody] EventDateTime eventDateTime) {
            try {
                bool isAdded = await _eventService.UpdateEventDateTime(userId, organizationId, eventId, eventDateTime);
                if (!isAdded) {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while updating eventDateTime.", exception);
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
