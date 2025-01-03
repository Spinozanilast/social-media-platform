using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoriesService.Common;
using StoriesService.Common.Mappers;
using StoriesService.Entities;
using StoriesService.Models;

namespace StoriesService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StoriesController(IStoryService storyService) : ControllerBase
    {
        private readonly IStoryService _storyService = storyService;

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStories([FromQuery] string? tag = null,
            [FromQuery] Guid? authorId = null, int pageNumber = 1, int pageSize = 10)
        {
            return Ok(await _storyService.GetAllStoriesAsync(tag, authorId, pageNumber, pageSize));
        }
        
        [AllowAnonymous]
        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStoriesCount([FromQuery] string? tag = null,
            [FromQuery] Guid? authorId = null)
        {
            return Ok(await _storyService.GetAllStoriesCountAsync(tag, authorId));
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStoryById([FromRoute] int id)
        {
            var story = await _storyService.GetStoryByIdAsync(id);
            if (story == null)
            {
                return NotFound();
            }

            return Ok(story);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStory([FromBody] CreateStoryModel createStoryModel)
        {
            var story = createStoryModel.ToStory();
            var result = await _storyService.AddStoryAsync(story);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(GetStoryById), new { id = story.Id }, story);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStory([FromRoute] int id, [FromBody] UpdateStoryModel updateStoryModel)
        {
            var story = await _storyService.GetStoryByIdAsync(id);

            if (story is null)
            {
                return NotFound();
            }

            updateStoryModel.UpdateStory(story);
            var result = await _storyService.UpdateStoryAsync(story);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStory([FromRoute] int id)
        {
            await _storyService.DeleteStoryAsync(id);
            return NoContent();
        }
    }
}