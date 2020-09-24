using System;
using System.Collections.Generic;

namespace CMS.API.Campaign.Application.Models
{
    public class BannerSummary
    {
        public int Page { get; set; }
        public int Platform { get; set; }
        public string Location { get; set; }
        public int Priority { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public string BannerName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string LastUpdateUser { get; set; }
        public int CampaignId { get; set; }
        public int CampaignTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> AvailableCountries { get; set; }
        public string CategoryId { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string HtmlWrapper { get; set; }
        public List<KeyValuePair<string, string>> Properties { get; set; }
        public string BackgroundUrl { get; set; }
        public string GeneratedImageUrl { get; set; }
        public string BackgroundColor { get; set; }
        public int ImageVersionId { get; set; }
    }
}
