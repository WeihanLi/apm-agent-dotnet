using CMS.API.Campaign.Application.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace CMS.API.Campaign.UnitTests.Services
{
    public class CacheServiceTest
    {
        private readonly ICacheService _cacheService;

        public CacheServiceTest()
        {
            var memoryCacheMock = new Mock<IMemoryCache>();
            memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                .Returns(false);
            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(new Mock<ICacheEntry>().Object);
            _cacheService = new CacheService(memoryCacheMock.Object);
        }

        [Fact]
        public void GetOrCreateTest()
        {
            var expectedValue = "iHerb";
            var result = _cacheService.GetOrSet("Test", () => "iHerb");
            Assert.Equal(expectedValue, result);
        }
    }
}