using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Threading;

namespace CMS.API.Campaign.Infrastructure.Redis
{
    public class RedisAccess : IRedisAccess
    {
        private static RedisConfig _redisConfig;
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
            new Lazy<ConnectionMultiplexer>(SetupConnection, LazyThreadSafetyMode.PublicationOnly);
        public RedisAccess(IOptions<RedisConfig> redisOptions)
        {
            _redisConfig = redisOptions.Value;
        }

        public IDatabase GetDatabase()
        {
            var db = LazyConnection.Value.GetDatabase();
            return db;
        }

        private static ConnectionMultiplexer SetupConnection()
        {
            var configOptions = new ConfigurationOptions();
            configOptions.EndPoints.Add(_redisConfig.ConnectionString);
            configOptions.ConnectTimeout = 4000;
            configOptions.SyncTimeout = 20000;
            configOptions.ReconnectRetryPolicy = new ExponentialRetry(5000);
            configOptions.AbortOnConnectFail = false;
            return ConnectionMultiplexer.Connect(configOptions);
        }
    }
}
