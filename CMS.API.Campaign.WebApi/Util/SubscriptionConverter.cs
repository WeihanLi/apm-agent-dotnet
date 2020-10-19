using iHerb.CMS.Cache.Redis;
using iHerb.CMS.Cache.Redis.Model;
using Newtonsoft.Json;

namespace CMS.API.Campaign
{
    internal sealed class CampaignSubscriptionConverter : ISubscriptionConverter
    {
        public KeyUpdateInfo[] Convert(string message)
        {
            return JsonConvert.DeserializeObject<KeyUpdateInfo[]>(message);
        }
    }
}