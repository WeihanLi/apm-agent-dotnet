namespace CMS.API.Campaign.Domain.Entities
{
    public class SlotEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string  Subtitle { get; set; }

        public string LandingUrl { get; set; }

        public string HtmlLayout { get; set; }

        public string CountrySpecific { get; set; }

        public string CountryExclude { get; set; }

        public string CategoryId { get; set; }

        public string PromoCode { get; set; }

        public string AltText { get; set; }

        public string EndDate { get; set; }
    }
}
