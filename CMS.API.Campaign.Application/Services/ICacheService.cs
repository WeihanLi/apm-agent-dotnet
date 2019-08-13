using CMS.API.Campaign.Application.Models;
using System.Collections.Generic;

namespace CMS.API.Campaign.Application.Services
{
    public interface ICacheService
    {
        void Set(string key, List<SlotInfo> slots);

        List<SlotInfo> Get(string key);
    }
}
