using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Application.Services;
using CMS.API.Campaign.Infrastructure.Metric;
using CMS.API.Campaign.WebApi.Controllers;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CMS.API.Campaign.UnitTests.Controllers
{
    public class SlotControllerTest
    {
        private readonly SlotController _slotController;

        public SlotControllerTest()
        {
            var slotService = new Mock<ISlotService>();
            var metricClient = new Mock<IMetricClient>();

            _slotController = new SlotController(metricClient.Object, slotService.Object);

            slotService.Setup(x => x.GetSlots(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new List<SlotInfo>()
                {
                    new SlotInfo()
                    {
                        AltText="", Id=1, Title="Big Sell", EndDate= "2019-09-01 22:00:00", HtmlLayout="<div></div>", Subtitle="", Url="/c/"
                    }
                });
        }

        [Fact]
        public void GetSlotsTest()
        {
            var result = _slotController.GetSlots("website", "topBanner", "en-us", "us", "", "", "");

            Assert.Equal("Big Sell", result.Value[0].Title);
        }

        [Fact]
        public void GetPreviewSlotsTest()
        {
            var result = _slotController.GetPreviewSlots("website", "topBanner", "en-us", "us", "", "", "");

            Assert.Equal("Big Sell", result.Value[0].Title);
        }
    }
}
