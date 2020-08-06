using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.API.Campaign.WebApi.Requests;
using CMS.API.Campaign.Application.Services;
using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Domain.Params;
using CMS.API.Campaign.WebApi.Util;
using System.Net;

namespace CMS.API.Campaign.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService;
        private readonly IValidator<GetBannerSummariesRequest> _getCampaignBannerValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<BannerController> _logger;

        public BannerController(IBannerService bannerService,
            IValidator<GetBannerSummariesRequest> getCampaignBannerValidator,
            IMapper mapper,
            ILogger<BannerController> logger)
        {
            _bannerService = bannerService;
            _getCampaignBannerValidator = getCampaignBannerValidator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public JsonSpecResult<List<BannerSummary>> GetBannerSummaries([FromQuery] GetBannerSummariesRequest getBannerRequest)
        {
            var validationResult = _getCampaignBannerValidator.Validate(getBannerRequest);
            if (!validationResult.IsValid)
            {
                var errorMessage = validationResult.Errors.Count > 0
                    ? string.Join(", ", validationResult.Errors.Select(i => i.ErrorMessage).ToArray())
                    : "Invalid request";
                return new JsonSpecResult<List<BannerSummary>>(errorMessage, (int)HttpStatusCode.BadRequest);
            }

            try
            {
                var result = _bannerService.GetCampaignBanners(
                    _mapper.Map<GetBannerSummariesRequest, GetBannerSummaries>(getBannerRequest));
                return new JsonSpecResult<List<BannerSummary>>(new JsonSpecDto { Data = result });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "BannerController.GetBannerSummaries");
                return new JsonSpecResult<List<BannerSummary>>(e.Message, (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
