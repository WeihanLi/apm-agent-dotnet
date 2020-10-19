using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Application.Services;
using CMS.API.Campaign.Domain.Params;
using CMS.API.Campaign.Infrastructure.Redis;
using iHerb.CMS.Cache.Redis;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace CMS.API.Campaign.UnitTests.Services
{
    public class BannerServiceTest
    {
        private readonly IBannerService _bannerService;
        private readonly Mock<IRedisCache> _redisMock = new Mock<IRedisCache>();

        public BannerServiceTest()
        {
            var banners = new List<BannerSummary>()
            {
                new BannerSummary()
                {
                    Active = true,
                    CampaignId = 1,
                    CampaignTypeId = 1,
                    BannerName = "test banner",
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(7),
                    AvailableCountries = new List<string>()
                    {
                        "US",
                        "CN"
                    }
                },
                new BannerSummary()
                {
                    Active = true,
                    CampaignId = 1,
                    CampaignTypeId = 1,
                    CategoryId = "1",
                    BannerName = "test banner(expired)",
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now.AddDays(-7),
                    AvailableCountries = new List<string>()
                    {
                        "US",
                        "CN"
                    }
                },
            };
            //
            _redisMock.Setup(x => x.GetCache(It.Is<string>(s => !string.IsNullOrEmpty(s)), It.IsAny<string>()))
                .Returns((string)null);
            _redisMock.Setup(x => x.GetCache(It.Is<string>(s => !string.IsNullOrEmpty(s)), It.IsAny<string>()))
                .Returns(JsonConvert.SerializeObject(banners));

            _bannerService = new BannerService(_redisMock.Object, Options.Create(new RedisConfig()
            {
                CampaignBannerSetName = "CMS-LatestCampaignBanner"
            }));
        }

        [Fact]
        public void GetCampaignBanners_InvalidConfig()
        {
            var result = new BannerService(_redisMock.Object, Options.Create(new RedisConfig())).GetCampaignBanners(new GetBannerSummaries());
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetCampaignBanners()
        {
            var result = _bannerService.GetCampaignBanners(new GetBannerSummaries());
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);
        }

        [Fact]
        public void GetCampaignBanners_WithQuery()
        {
            var result = _bannerService.GetCampaignBanners(new GetBannerSummaries()
            {
                CategoryId = "1"
            });
            Assert.NotNull(result);
            Assert.Empty(result);

            result = _bannerService.GetCampaignBanners(new GetBannerSummaries()
            {
                Country = "US"
            });
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);

            result = _bannerService.GetCampaignBanners(new GetBannerSummaries()
            {
                Country = "JP"
            });
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}