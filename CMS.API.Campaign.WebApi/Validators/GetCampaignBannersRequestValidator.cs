using CMS.API.Campaign.WebApi.Requests;
using FluentValidation;

namespace CMS.API.Campaign.WebApi.Validators
{
    public class GetCampaignBannersRequestValidator : AbstractValidator<GetBannerSummariesRequest>
    {
        public GetCampaignBannersRequestValidator()
        {
            RuleFor(x => x.Platform).NotNull();
            RuleFor(x => x.Page).NotNull();
            RuleFor(x => x.Module).NotNull().NotEmpty();
            RuleFor(x => x.Language).NotNull().NotEmpty();
        }
    }
}