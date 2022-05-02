using StackExchange.Redis;

namespace ConnectionHandler.Redis.Interfaces.Base
{
    public interface IDatabaseProvider
    {
        IDatabase GetDatabase();
        ISubscriber GetSubscribers();
    }
}
