using CMS.API.Campaign.Application.Models;
using iHerb.CMS.Cache.Redis;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CMS.API.Campaign.WebApi.Controllers
{
    [Route("api/checks/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IRedisCache<List<BannerSummary>> _redisCache;

        public HealthController(IRedisCache<List<BannerSummary>> redisCache)
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
            if (_redisCache.Readiness())
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}