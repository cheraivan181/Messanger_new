using StackExchange.Redis;

namespace ConnectionHandler.Redis.Interfaces.Base
{
    public interface IMultiplexerProvider
    {
        ConnectionMultiplexer ConnectionMultiplexer { get; }
        Lazy<ConnectionMultiplexer> LazyConnectionMultiplexer { get; }
        void RenewConnectionMultiplexer();
    }
}
