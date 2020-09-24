using System;
using AutoMapper;
using Params = CMS.API.Campaign.Domain.Params;

namespace CMS.API.Campaign.WebApi.Util
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Requests.GetBannerSummariesRequest, Params.GetBannerSummaries>()
                .ForMember(des => des.Language, opts => opts.MapFrom(src => src.Language.ToLower()));
        }
    }
}
