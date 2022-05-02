using ConnectionHandler.Redis.Domain.Options;
using ConnectionHandler.Redis.Interfaces.Base;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ConnectionHandler.Redis.Implementations.Base
{
    public class MultiplexerProvider : IMultiplexerProvider
    {
        private readonly IOptions<RedisOptions> _redisOptions;
        private Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        public MultiplexerProvider(IOptions<RedisOptions> redisOptions)
        {
            _redisOptions = redisOptions;
        }

        private Lazy<ConnectionMultiplexer> CreateConnection()
        {
            return new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(_redisOptions.Value.Host);
            });
        }
        public ConnectionMultiplexer ConnectionMultiplexer
        {
            get
            {
                if (_connectionMultiplexer == null)
                {
                    _connectionMultiplexer = CreateConnection();
                }
                return _connectionMultiplexer.Value;
            }
        }

        public Lazy<ConnectionMultiplexer> LazyConnectionMultiplexer
        {
            get
            {
                return _connectionMultiplexer;
            }
        }

        public void RenewConnectionMultiplexer()
        {
            _connectionMultiplexer = CreateConnection();
        }
    }
}
