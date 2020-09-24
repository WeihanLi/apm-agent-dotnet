using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Domain.Params;
using System.Collections.Generic;

namespace CMS.API.Campaign.Application.Services
{
    public interface IBannerService
    {
        List<BannerSummary> GetCampaignBanners(GetBannerSummaries campaignBannerDto);
    }
}
