using System;
using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Application.Services;
using CMS.API.Campaign.Domain.Entities;
using CMS.API.Campaign.Domain.Repositories;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CMS.API.Campaign.UnitTests.Services
{
    public class SlotServiceTest
    {
        private readonly ISlotService _slotService;

        public SlotServiceTest()
        {
            var slotRepository = new Mock<ISlotRepository>();
            var cacheRepository = new Mock<ICacheRepository>();

            slotRepository
                .Setup(x => x.GetSlots(It.IsAny<string>(), false))
                .Returns(new List<SlotEntity>()
                {
                    new SlotEntity()
                    {
                        Id = 1, Title = "test1", CountryExclude = "UK,AU,", CountrySpecific = "Global",
                        LandingUrl = "/c",
                        HtmlLayout = "<div><img src=https://s3.images-iherb.com/cms/banners/suspbanner0605.jpg /></div>"
                    },
                    new SlotEntity
                    {
                        Id = 2, Title = "test2", CountryExclude = "", CountrySpecific = "US,CN, AU",
                        AltText = "big sell", EndDate = DateTime.Parse("2019-09-01 22:00:00")
                    }
                });

            cacheRepository.Setup(x => x.Get(It.IsAny<string>())).Returns(new List<SlotEntity>());


            _slotService = new SlotService(slotRepository.Object, new OptionsWrapper<SlotImageConfig>(
                new SlotImageConfig()
                {
                    DefaultHeader = "https://s3.images-iherb.com", ChinaHeader = "https://s3.iherb.cn"
                }), cacheRepository.Object);
        }

        [Fact]
        public void GetSlots_Test()
        {
            var res = _slotService.GetSlots("website", "homePagePromo", "US", "en-US", "", "", "iHerb");

            Assert.Equal("<div><img src=https://s3.images-iherb.com/cms/banners/suspbanner0605.jpg /></div>",
                res[0].HtmlLayout);
            Assert.Null(res[1].HtmlLayout);
        }

        [Fact]
        public void GetSlots_Test2()
        {
            var res = _slotService.GetSlots("website", "homePagePromo", "CN", "cn-ZH", "", "", "iHerb");

            Assert.Equal("<div><img src=https://s3.iherb.cn/cms/banners/suspbanner0605.jpg /></div>",
                res[0].HtmlLayout);
        }

        [Fact]
        public void GetSlots_Test3()
        {
            var res = _slotService.GetSlots("website", "homePagePromo", "AU", "en-US", "", "", "iHerb");

            Assert.Equal("2019-09-01 15:00:00", res[0].EndDate);
        }
    }
}
