using StackExchange.Redis;

namespace CMS.API.Campaign.Infrastructure.Redis
{
    public interface IRedisAccess
    {
        IDatabase GetDatabase();
    }
}