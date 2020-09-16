using AutoMapper;
using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Application.Services;
using CMS.API.Campaign.Domain.Params;
using CMS.API.Campaign.WebApi.Controllers;
using CMS.API.Campaign.WebApi.Requests;
using CMS.API.Campaign.WebApi.Util;
using CMS.API.Campaign.WebApi.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CMS.API.Campaign.UnitTests.Controllers
{
    public class BannerControllerTest
    {
        private readonly BannerController _controller;

        public BannerControllerTest()
        {
            var bannerMock = new Mock<IBannerService>();
            bannerMock.Setup(x => x.GetCampaignBanners(It.IsAny<GetBannerSummaries>()))
                .Returns(new List<BannerSummary>()
                {
                    new BannerSummary()
                });
            var mapper = new Mapper(new MapperConfiguration(configure =>
            {
                configure.AddProfile<AutomapperProfile>();
            }));
            _controller = new BannerController(bannerMock.Object, new GetCampaignBannersRequestValidator(), mapper, NullLogger<BannerController>.Instance);
        }

        [Fact]
        public void GetBannerSummaries_InvalidRequest()
        {
            var result = _controller.GetBannerSummaries(new GetBannerSummariesRequest());
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(result.Result.Error);
            Assert.NotEmpty(result.Result.Error);
        }

        [Fact]
        public void GetBannerSummaries()
        {
            var result = _controller.GetBannerSummaries(new GetBannerSummariesRequest()
            {
                Platform = 1,
                Page = 1,
                Country = "CN",
                Language = "zh-CN",
                Module = "Header"
            });
            Assert.NotNull(result?.Result?.Data);
            Assert.Equal(200, result.StatusCode);
            Assert.Empty(result.Result.Error);
        }
    }
}