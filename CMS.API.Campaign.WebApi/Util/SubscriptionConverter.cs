using iHerb.CMS.Cache.Redis;
using iHerb.CMS.Cache.Redis.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using RedisConfig = CMS.API.Campaign.Infrastructure.Redis.RedisConfig;

namespace CMS.API.Campaign
{
    internal class CampaignSubscriptionConverter : ISubscriptionConverter
    {
        private readonly string _setName;

        public CampaignSubscriptionConverter(IOptions<RedisConfig> options)
        {
            _setName = options.Value.CampaignBannerSetName;
        }

        public KeyUpdateInfo[] Convert(string message)
        {
            if (string.IsNullOrEmpty(message))
                return Array.Empty<KeyUpdateInfo>();

            var info = JsonConvert.DeserializeObject<KeyUpdateInfo[]>(message);
            foreach (var item in info)
            {
                item.SetName = _setName;
            }
            return info;
        }
    }
}