using Microsoft.Extensions.Caching.Memory;
using System;

namespace CMS.API.Campaign.Application.Services
{
    public interface ICacheService
    {
        T GetOrSet<T>(string entry, Func<T> factory, int cacheSize = 1,
            CacheItemPriority priority = CacheItemPriority.Normal);
    }
}