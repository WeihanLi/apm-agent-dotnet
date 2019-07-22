using CMS.API.Campaign.Domain.Entities;
using System.Collections.Generic;

namespace CMS.API.Campaign.Domain.Repositories
{
    public interface ISlotRepository
    {
        List<SlotEntity> GetSlots(string platform, string location, string language, string store, bool preview = false);
    }
}
