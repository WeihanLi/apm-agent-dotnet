using CMS.API.Campaign.Application.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace CMS.API.Campaign.Application.Services
{
    public interface ICacheService
    {
        void Set(string key, List<SlotInfo> slots);
        List<SlotInfo> Get(string key);
        T GetOrSet<T>(string entry, Func<T> factory, int cacheSize = 1,
            CacheItemPriority priority = CacheItemPriority.Normal);
    }
}
