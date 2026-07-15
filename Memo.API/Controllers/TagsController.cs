using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Memo.Application.Interfaces;

namespace Memo.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularTags([FromQuery] int limit = 10)
        {
            var tags = await _tagService.GetPopularTagsAsync(limit);
            return Ok(tags);
        }
    }
}