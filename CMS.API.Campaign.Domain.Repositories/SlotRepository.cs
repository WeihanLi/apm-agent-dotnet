using System;
using CMS.API.Campaign.Domain.Entities;
using CMS.API.Campaign.Infrastructure.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Diagnostics;

namespace CMS.API.Campaign.Domain.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly IRedisAccess _redisAccess;

        public SlotRepository(IRedisAccess redisAccess)
        {
            _redisAccess = redisAccess;
        }

        public List<SlotEntity> GetSlots(string key, bool preview = false)
        {
            var result = GetValue(preview ? "CMS-PreviewCampaigns" : "CMS-Campaigns", key);

            return string.IsNullOrEmpty(result)
                ? new List<SlotEntity>()
                : JsonConvert.DeserializeObject<List<SlotEntity>>(result);
        }

        private string GetValue(string setName, RedisValue key)
        {
            var stop = new Stopwatch();
            stop.Start();
            var db = _redisAccess.GetDatabase();
            var value = db.HashGet(setName, key);
            stop.Stop();
            Console.WriteLine($"Read from redis by key = {key}, elapsed = {stop.ElapsedMilliseconds}ms.");
            return value.HasValue ? value.ToString() : null;
        }
    }
}