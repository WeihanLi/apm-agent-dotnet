using CMS.API.Campaign.Domain.Entities;
using CMS.API.Campaign.Infrastructure.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;

namespace CMS.API.Campaign.Domain.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly IRedisAccess _redisAccess;

        public SlotRepository(IRedisAccess redisAccess)
        {
            _redisAccess = redisAccess;
        }

        public List<SlotEntity> GetSlots(string platform, string location, string language, string store, bool preview = false)
        {
            var key = $"{store.ToLower()}-{platform.ToLower()}-{location.ToLower()}-{language.ToLower()}";

            var result = GetValue(preview ? "CMS-PreviewCampaigns" : "CMS-Campaigns", key);

            return string.IsNullOrEmpty(result)
                ? new List<SlotEntity>()
                : JsonConvert.DeserializeObject<List<SlotEntity>>(result);
        }

        private string GetValue(string setName, RedisValue key)
        {
            var db = _redisAccess.GetDatabase();
            var value = db.HashGet(setName, key);
            return value.HasValue ? value.ToString() : null;
        }
    }
}