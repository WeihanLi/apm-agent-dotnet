using CMS.API.Campaign.Application.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace CMS.API.Campaign.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Random _random = new Random();
        private const int TimeoutBaselineInMinutes = 10;
        private const int TimeoutVarietyInMinutes = 5;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set(string key, List<SlotInfo> slots)
        {
            _memoryCache.Set(key, slots, TimeSpan.FromSeconds(30));
        }

        public List<SlotInfo> Get(string key)
        {
            return _memoryCache.Get<List<SlotInfo>>(key);
        }

        public T GetOrSet<T>(string entry, Func<T> factory, int cacheSize = 1,
            CacheItemPriority priority = CacheItemPriority.Normal)
        {
            if (!_memoryCache.TryGetValue(entry, out T cacheEntry))
            {
                cacheEntry = factory();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(cacheSize)
                    .SetPriority(priority)
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(TimeoutBaselineInMinutes + TimeoutVarietyInMinutes * _random.NextDouble()));
                _memoryCache.Set(entry, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }
    }
}
