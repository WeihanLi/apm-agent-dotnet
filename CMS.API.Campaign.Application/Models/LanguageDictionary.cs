using System.Collections.Generic;

namespace CMS.API.Campaign.Application.Models
{
    public class LanguageDictionary
    {
        public string LanguageCode { get; set; }
        public List<Entry> Entries { get; set; }
    }
}
