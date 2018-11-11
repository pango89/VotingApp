using StackExchange.Redis;

namespace worker
{
    public interface IRedisStore
    {
        ConnectionMultiplexer GetConnection();
    }
}