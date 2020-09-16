using Microsoft.Extensions.Caching.Memory;
using System;

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