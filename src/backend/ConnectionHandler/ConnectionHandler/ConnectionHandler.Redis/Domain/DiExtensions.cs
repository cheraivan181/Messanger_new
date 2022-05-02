using ConnectionHandler.Redis.Domain.Options;
using ConnectionHandler.Redis.Implementations.Base;
using ConnectionHandler.Redis.Interfaces.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectionHandler.Redis.Domain
{
    public static class DiExtensions
    {
        public static void AddRedisServices(this IServiceCollection serviceProvider,
            IConfiguration configuration)
        {
            serviceProvider.Configure<RedisOptions>(configuration.GetSection("Redis"));
           
            serviceProvider.AddSingleton<IMultiplexerProvider, MultiplexerProvider>();
            serviceProvider.AddSingleton<IDatabaseProvider, DatabaseProvider>();
        }
    }
}
