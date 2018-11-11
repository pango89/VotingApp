using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace worker
{
    public class RedisStore : IRedisStore
    {
        private readonly IConfiguration _configuration;

        public RedisStore(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public ConnectionMultiplexer GetConnection()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { _configuration.GetValue<string>("Redis:Url") }
            };

            var connection = ConnectionMultiplexer.Connect(configurationOptions);

            return connection;
        }
    }
}
