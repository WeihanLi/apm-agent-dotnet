using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.WebApi.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CMS.API.Campaign.UnitTests.Util
{
    public class JsonSpecResultUT
    {
        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonSpecResult<BannerSummary>((JsonSpecDto)null));
        }

        [Fact]
        public async Task ExecuteTest()
        {
            var mockExecutor = new Mock<IActionResultExecutor<JsonResult>>();
            mockExecutor.Setup(x => x.ExecuteAsync(It.IsAny<ActionContext>(), It.IsAny<JsonResult>()))
                .Returns(Task.CompletedTask);

            var actionContext = new ActionContext(new DefaultHttpContext(new FeatureCollection())
            {
                RequestServices = new ServiceCollection().AddSingleton<IActionResultExecutor<JsonResult>>(mockExecutor.Object).BuildServiceProvider()
            }, new RouteData(), new ActionDescriptor());

            var result = new JsonSpecResult<BannerSummary>(new JsonSpecDto());
            await result.ExecuteResultAsync(actionContext);
        }
    }
}