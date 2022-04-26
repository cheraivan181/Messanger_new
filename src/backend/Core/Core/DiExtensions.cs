using Core.CacheServices.Domain;
using Core.CryptProtocol;
using Core.CryptService;
using Core.DbModels.Base.Di;
using Core.DialogServices;
using Core.IdentityService;
using Core.Kafka;
using Core.Mapping;
using Core.MessageServices;
using Core.Repositories;
using Core.SearchServices;
using Core.SenderServices;
using Core.SessionServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class DiExtensions
    {
        public static void AddAppInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbServices();
            serviceCollection.AddRepositories();
            serviceCollection.AddIdentityServices(configuration);
            serviceCollection.AddMappers();
            serviceCollection.AddCryptService();
            serviceCollection.AddRedisServices(configuration);
            serviceCollection.AddKafkaServices(configuration);
            serviceCollection.AddSessionServices();
            serviceCollection.AddSearchUserServices();
            serviceCollection.AddDialogServices();
            serviceCollection.AddProtocolServices();
            serviceCollection.AddMessageServices();
            serviceCollection.AddSenderServices();
        } 
    }
}
