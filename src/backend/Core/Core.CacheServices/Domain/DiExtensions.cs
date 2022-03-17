using Core.CacheServices.Implementations;
using Core.CacheServices.Implementations.Base;
using Core.CacheServices.Interfaces.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CacheServices.Domain
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
