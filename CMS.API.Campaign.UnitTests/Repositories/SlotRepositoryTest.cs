using CMS.API.Campaign.Domain.Entities;
using CMS.API.Campaign.Domain.Repositories;
using CMS.API.Campaign.Infrastructure.Redis;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using Xunit;

namespace CMS.API.Campaign.UnitTests.Repositories
{
    public class SlotRepositoryTest
    {
        private readonly ISlotRepository _slotRepository;

        public SlotRepositoryTest()
        {
            var redisAccess = new Mock<IRedisAccess>();
            var databaseMock = new Mock<IDatabase>();
            
            redisAccess.Setup(x => x.GetDatabase()).Returns(databaseMock.Object);
            
            var list = new List<SlotEntity>()
            {
                new SlotEntity()
                {
                    Id=1, Title="test"
                },
                new SlotEntity()
                {
                    Id=2, Title="test2"
                }
            };

            databaseMock.Setup(x => x.HashGet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), CommandFlags.None)).Returns(JsonConvert.SerializeObject(list));
            
            _slotRepository = new SlotRepository(redisAccess.Object);
        }

        [Fact]
        public void GetSlots_Test()
        {
            var res = _slotRepository.GetSlots("iherb-website-homepagepromo-en-us");

            Assert.NotNull(res);
            Assert.True(res.Count>0);
            Assert.Equal("test", res[0].Title);
        }
    }
}
