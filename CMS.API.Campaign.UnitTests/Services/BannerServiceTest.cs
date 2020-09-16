using CMS.API.Campaign.Application.Services;
using CMS.API.Campaign.Domain.Params;
using CMS.API.Campaign.Infrastructure.Redis;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace CMS.API.Campaign.UnitTests.Services
{
    public class BannerServiceTest
    {
        private readonly IBannerService _bannerService;

        public BannerServiceTest()
        {
            var redisMock = new Mock<IRedisAccess>();
            var redisDbMock = new Mock<IDatabase>();
            redisDbMock.Setup(x => x.HashGet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
                .Returns(new RedisValue());
            redisMock.Setup(x => x.GetDatabase()).Returns(redisDbMock.Object);

            var memoryCacheMock = new Mock<IMemoryCache>();
            memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                .Returns(false);
            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(new Mock<ICacheEntry>().Object);
            var cacheService = new CacheService(memoryCacheMock.Object);

            _bannerService = new BannerService(redisMock.Object, cacheService, Options.Create(new RedisConfig()));
        }

        [Fact]
        public void GetCampaignBanners()
        {
            var result = _bannerService.GetCampaignBanners(new GetBannerSummaries());
            Assert.NotNull(result);
        }
    }
}