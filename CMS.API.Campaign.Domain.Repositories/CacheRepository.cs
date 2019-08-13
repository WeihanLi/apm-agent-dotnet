using CMS.API.Campaign.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace CMS.API.Campaign.Domain.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IMemoryCache _memoryCache;
        public CacheRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set(string key, List<SlotEntity> slots)
        {
            _memoryCache.Set(key, slots, TimeSpan.FromSeconds(60));
        }

        public List<SlotEntity> Get(string key)
        {
            return _memoryCache.Get<List<SlotEntity>>(key);
        }
    }
}
