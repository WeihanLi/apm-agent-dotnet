using CMS.API.Campaign.Infrastructure.Common;

namespace CMS.API.Campaign.WebApi.Requests
{
    /// <summary>
    /// platform: 1-Desktop, 2-Mobile, 3-GlobalApp, 4-ChinaApp
    /// page: 0 - Universal, 1 - Home Page, 2 - Store Page, 3 - Special Promo, 4 - About Us, 5 - Reward, 6 - Blog, 7 - Trial, 8 - Shipping, 9 - Special Deal
    /// </summary>
    public class GetBannerSummariesRequest
    {
        public int? Page { get; set; }
        public int? Platform { get; set; }
        public string Module { get; set; }  //i.e. location
        public string Language { get; set; }
        public StoreIdEnum? Store { get; set; }
        public string Country { get; set; }
        public int? CategoryId { get; set; }
        public int? PromoId { get; set; }
        public GetBannerSummariesRequest()
        {
            Store = null;
            Country = null;
            CategoryId = null;
            PromoId = null;
        }
    }
}
