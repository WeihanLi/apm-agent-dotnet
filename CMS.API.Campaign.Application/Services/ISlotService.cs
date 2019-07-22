using CMS.API.Campaign.Application.Models;
using System.Collections.Generic;

namespace CMS.API.Campaign.Application.Services
{
    public interface ISlotService
    {
        List<SlotInfo> GetSlots(string platform, string location, string country, string language,
            string categoryId, string promoId, string store, bool preview = false);
    }
}
