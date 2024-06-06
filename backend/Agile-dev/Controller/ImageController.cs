using agile_dev.Models;
using agile_dev.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agile_dev.Controller {
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase {
        private readonly ImageService _imageService;

        #region GET
        
        // GET: api/image/fetchAll
        [HttpGet("fetchAll")]
        public async Task<ActionResult> FetchAllImages() {
            try {
                ICollection<Image> result = await _imageService.FetchAllImages();
                if (result.Count == 0) {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception exception) {
                return StatusCode(500, "Internal server Error: " + exception.Message);
            }
        }

        // GET api/image/fetch/id/5
        [HttpGet("/fetch/id/{id}")]
        public async Task<IActionResult> Get(int id) {
            try {
                Image? result = await _imageService.FetchImageById(id);
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

        // POST api/image/create/5/6
        [HttpPost("create/{userId}/{organizationId}")]
        public async Task<IActionResult> AddImage([FromRoute] int userId, [FromBody] Image image, [FromRoute] int organizationId) {
            try {
                bool isAdded = await _imageService.AddImage(userId, organizationId, image);
                if (!isAdded) {
                    // Could not create image, because request is bad
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while adding organization to database.", exception);
            }
        }
        
        #endregion

        #region PUT

        // PUT api/image/update/5/6
        [HttpPut("update/{userId}/{organizationId}")]
        public async Task<IActionResult> UpdateEvent([FromRoute] int userId, [FromRoute] int organizationId, [FromBody] Image image) {
            try {
                bool isAdded = await _imageService.UpdateImage(userId, organizationId, image);
                if (!isAdded) {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while updating event.", exception);
            }
        }
        
        #endregion

        #region DELETE

        // DELETE api/image/delete/5/6
        [HttpDelete("delete/{userId}/{organizationId}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int userId, [FromRoute] int organizationId, [FromBody] Image image) {
            try {
                bool isDeleted = await _imageService.DeleteImage(userId, organizationId, image);
                if (!isDeleted) {
                    // Could not delete image, because request is bad
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
