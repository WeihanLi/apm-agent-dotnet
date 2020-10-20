using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Domain.Params;
using CMS.API.Campaign.Infrastructure.Common;
using CMS.API.Campaign.Infrastructure.Redis;
using iHerb.CMS.Cache.Redis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS.API.Campaign.Application.Services
{
    public class BannerService : IBannerService
    {
        private readonly IRedisCache<List<BannerSummary>> _redisCache;
        private readonly string _campaignBannerSetName;

        public BannerService(IRedisCache<List<BannerSummary>> redisCache, IOptions<RedisConfig> redisOptions)
        {
            _redisCache = redisCache;
            _campaignBannerSetName = redisOptions.Value.CampaignBannerSetName;
        }

        public List<BannerSummary> GetCampaignBanners(GetBannerSummaries campaignBannerDto)
        {
            var cacheEntry = GetCampaignBannerSetKey(campaignBannerDto.Page ?? -1, campaignBannerDto.Platform ?? -1, campaignBannerDto.Module, campaignBannerDto.Language);
            var result = _redisCache.GetCache(_campaignBannerSetName, cacheEntry);
            return FilterBannerSummaryListByDateAndOptionalParam(result, campaignBannerDto.Store,
                campaignBannerDto.Country, campaignBannerDto.CategoryId, null);
        }

        private static string GetCampaignBannerSetKey(int page, int platform, string location, string language)
        {
            return $"{page}-{platform}-{location}-{language}";
        }

        private static List<BannerSummary> FilterBannerSummaryListByDateAndOptionalParam(List<BannerSummary> bannerSummaries, StoreIdEnum? store, string country, string categoryId, int? promoId)
        {
            if (bannerSummaries == null || bannerSummaries.Count == 0)
                return bannerSummaries ?? new List<BannerSummary>();

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