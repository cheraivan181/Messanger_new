using StackExchange.Redis;

namespace Core.CacheServices.Interfaces.Base
{
    public interface IMultiplexerProvider
    {
        ConnectionMultiplexer ConnectionMultiplexer { get; }
        Lazy<ConnectionMultiplexer> LazyConnectionMultiplexer { get; }
        void RenewConnectionMultiplexer();
    }
}
