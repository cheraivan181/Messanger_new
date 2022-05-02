using StackExchange.Redis;
using System.Net.Sockets;
using ConnectionHandler.Redis.Interfaces.Base;

namespace ConnectionHandler.Redis.Implementations.Base
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IMultiplexerProvider _multiplexerProvider;

        public DatabaseProvider(IMultiplexerProvider multiplexerProvider)
        {
            _multiplexerProvider = multiplexerProvider;
        }

        private long lastReconnectTicks = DateTimeOffset.MinValue.UtcTicks;
        private DateTimeOffset firstErrorTime = DateTimeOffset.MinValue;
        private DateTimeOffset previousErrorTime = DateTimeOffset.MinValue;

        private readonly object reconnectLock = new object();

        private TimeSpan ReconnectMinFrequency => TimeSpan.FromSeconds(60);

        private TimeSpan ReconnectErrorThreshold => TimeSpan.FromSeconds(30);

        private static int RetryMaxAttempts => 5;

        public IDatabase GetDatabase()
        {
            return BasicRetry(() => _multiplexerProvider.ConnectionMultiplexer.GetDatabase());
        }

        public ISubscriber GetSubscribers()
        {
            return BasicRetry(() => _multiplexerProvider.ConnectionMultiplexer.GetSubscriber());
        }

        private T BasicRetry<T>(Func<T> func)
        {
            int reconnectRetry = 0;
            int disposedRetry = 0;

            while (true)
            {
                try
                {
                    return func();
                }
                catch (Exception ex) when (ex is RedisConnectionException || ex is SocketException)
                {
                    reconnectRetry++;
                    if (reconnectRetry > RetryMaxAttempts)
                        throw;
                    ForceReconnect();
                }
                catch (ObjectDisposedException)
                {
                    disposedRetry++;
                    if (disposedRetry > RetryMaxAttempts)
                        throw;
                }
            }
        }

        private void CloseConnection(Lazy<ConnectionMultiplexer> oldConnection)
        {
            if (oldConnection == null)
                return;

            try
            {
                oldConnection.Value.Close();
            }
            catch (Exception)
            {
            }
        }

        public void ForceReconnect()
        {
            var utcNow = DateTimeOffset.UtcNow;
            long previousTicks = Interlocked.Read(ref lastReconnectTicks);
            var previousReconnectTime = new DateTimeOffset(previousTicks, TimeSpan.Zero);
            TimeSpan elapsedSinceLastReconnect = utcNow - previousReconnectTime;

            if (elapsedSinceLastReconnect < ReconnectMinFrequency)
                return;

            lock (reconnectLock)
            {
                utcNow = DateTimeOffset.UtcNow;
                elapsedSinceLastReconnect = utcNow - previousReconnectTime;

                if (firstErrorTime == DateTimeOffset.MinValue)
                {
                    firstErrorTime = utcNow;
                    previousErrorTime = utcNow;
                    return;
                }

                if (elapsedSinceLastReconnect < ReconnectMinFrequency)
                    return;

                TimeSpan elapsedSinceFirstError = utcNow - firstErrorTime;
                TimeSpan elapsedSinceMostRecentError = utcNow - previousErrorTime;

                bool shouldReconnect =
                    elapsedSinceFirstError >= ReconnectErrorThreshold
                    && elapsedSinceMostRecentError <= ReconnectErrorThreshold;
                previousErrorTime = utcNow;

                if (!shouldReconnect)
                    return;

                firstErrorTime = DateTimeOffset.MinValue;
                previousErrorTime = DateTimeOffset.MinValue;

                Lazy<ConnectionMultiplexer> oldConnection = _multiplexerProvider.LazyConnectionMultiplexer;
                CloseConnection(oldConnection);

                _multiplexerProvider.RenewConnectionMultiplexer();

                Interlocked.Exchange(ref lastReconnectTicks, utcNow.UtcTicks);
            }
        }
    }
}
