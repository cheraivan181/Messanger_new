using StackExchange.Redis;

namespace Core.CacheServices.Interfaces.Base
{
    public interface IDatabaseProvider
    {
        IDatabase GetDatabase();
        ISubscriber GetSubscribers();
    }
}
