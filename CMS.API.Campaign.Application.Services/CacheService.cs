using CMS.API.Campaign.Application.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace CMS.API.Campaign.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set(string key, List<SlotInfo> slots)
        {
            _memoryCache.Set(key, slots, TimeSpan.FromSeconds(60));
        }

        public List<SlotInfo> Get(string key)
        {
            return _memoryCache.Get<List<SlotInfo>>(key);
        }
    }
}
