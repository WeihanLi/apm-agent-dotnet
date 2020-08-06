using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Domain.Params;
using CMS.API.Campaign.Infrastructure.Common;
using CMS.API.Campaign.Infrastructure.Redis;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS.API.Campaign.Application.Services
{
    public class BannerService : IBannerService
    {
        private readonly ICacheService _cacheService;
        private readonly IRedisAccess _redisAccess;
        private readonly string _campaignBannerSetName;
        private const string MemoryCacheEntryPrefix = "CMS_BannerSvc_";

        public BannerService(IRedisAccess redisAccess, ICacheService cacheService, IOptions<RedisConfig> redisOptions)
        {
            _redisAccess = redisAccess;
            _cacheService = cacheService;
            _campaignBannerSetName = redisOptions.Value.CampaignBannerSetName;
        }

        public List<BannerSummary> GetCampaignBanners(GetBannerSummaries campaignBannerDto)
        {
            var cacheEntry = GetCampaignBannerSetKey(campaignBannerDto.Page ?? -1, campaignBannerDto.Platform ?? -1, campaignBannerDto.Module, campaignBannerDto.Language);
            var memoryCacheEntry = GetMemoryCacheKey(cacheEntry);

            var bannerSummaries = _cacheService.GetOrSet(memoryCacheEntry,
                () => GetFromRedisByHashField(cacheEntry), 2);
            return FilterBannerSummaryListByDateAndOptionalParam(bannerSummaries, campaignBannerDto.Store,
                campaignBannerDto.Country, campaignBannerDto.CategoryId, null);
        }

        private static string GetCampaignBannerSetKey(int page, int platform, string location, string language)
        {
            return $"{page}-{platform}-{location}-{language}";
        }

        private static string GetMemoryCacheKey(string cacheEntry)
        {
            return $"{MemoryCacheEntryPrefix}-{cacheEntry}";
        }

        private List<BannerSummary> GetFromRedisByHashField(string hashField)
        {
            var db = _redisAccess.GetDatabase();
            var redisValue = db.HashGet(_campaignBannerSetName, hashField);
            return redisValue.HasValue ?
                JsonConvert.DeserializeObject<List<BannerSummary>>(redisValue)
                : new List<BannerSummary>();
        }

        private static List<BannerSummary> FilterBannerSummaryListByDateAndOptionalParam(List<BannerSummary> bannerSummaries, StoreIdEnum? store, string country, string categoryId, int? promoId)
        {
            var query = from bs in bannerSummaries
                        where bs.StartDate < DateTime.Now && bs.EndDate > DateTime.Now
                        select bs;
            if (!string.IsNullOrEmpty(country))
            {
                query = from bs in query
                        where bs.AvailableCountries.Contains(country)
                        select bs;
            }

            if (!string.IsNullOrEmpty(categoryId))
            {
                query = from bs in query
                        where bs.CategoryId == categoryId
                        select bs;
            }

            return query.ToList();
        }
    }
}
