using iHerb.CMS.Cache.Redis;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Campaign.WebApi.Controllers
{
    [Route("api/checks/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IRedisCache _redisCache;

        public HealthController(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }

        /// <summary>
        /// HealthCheck
        /// </summary>
        [HttpGet]
        public IActionResult Health()
        {
            return Ok();
        }

        /// <summary>
        /// ReadinessCheck
        /// </summary>
        /// <returns>200: Ready, 400: NotReady</returns>
        [HttpGet]
        public IActionResult Readiness()
        {
            var ready = _redisCache.Readiness();
            if (ready)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}