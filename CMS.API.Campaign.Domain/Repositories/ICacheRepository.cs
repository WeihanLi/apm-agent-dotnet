using CMS.API.Campaign.Domain.Entities;
using System.Collections.Generic;

namespace CMS.API.Campaign.Domain.Repositories
{
    public interface ICacheRepository
    {
        void Set(string key, List<SlotEntity> slots);

        List<SlotEntity> Get(string key);
    }
}
