using CMS.API.Campaign.Infrastructure.Common;

namespace CMS.API.Campaign.Domain.Params
{
    public class GetBannerSummaries
    {
        public int? Platform { get; set; }
        public int? Page { get; set; }
        public string Module { get; set; }
        public string Language { get; set; }
        public StoreIdEnum? Store { get; set; }
        public string Country { get; set; }
        public string CategoryId { get; set; }
        public int? PromoId { get; set; }
    }
}
