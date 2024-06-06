using System.Security.Claims;
using agile_dev.Models;
using agile_dev.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agile_dev.Controller {
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase {
        private readonly ImageService _imageService;
        
        public ImageController(ImageService imageService) {
            _imageService = imageService;
        }

        #region GET
        
        // GET: api/image/fetchAll
        [Authorize]
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
        [Authorize]
        [HttpGet("fetch/id/{id}")]
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
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> AddImage(Image image) {
            try {
                bool isAdded = await _imageService.AddImage(image);
                if (!isAdded) {
                    // Could not create image, because request is bad
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception exception) {
                throw new Exception("An error occurred while adding image to database.", exception);
            }
        }
        
        #endregion

        #region DELETE

        // DELETE api/image/delete/5/6
        [Authorize]
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
