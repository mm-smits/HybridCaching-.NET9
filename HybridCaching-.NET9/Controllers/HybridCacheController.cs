using HybridCaching.Services;
using Microsoft.AspNetCore.Mvc;

namespace HybridCaching.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HybridCacheController : ControllerBase
    {
        private readonly HybridCacheService _hybridCacheService;
        public HybridCacheController(HybridCacheService hybridCacheService)
        {
            _hybridCacheService = hybridCacheService;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetCache(string key)
        {
            var cachedData = await _hybridCacheService.GetCachedDataAsync(key);
            if (cachedData == null)
            {
                return NotFound("No cache entry found.");
            }
            return Ok(cachedData);
        }

        // POST: hybridcache/{key}
        [HttpPost("{key}")]
        public async Task<IActionResult> SetCache(string key, [FromBody] string value)
        {
            await _hybridCacheService.SetCacheDataAsync(key, value);
            return Ok("Cache entry created/updated.");
        }
    }
}
