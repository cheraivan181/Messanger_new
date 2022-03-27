﻿using Core.CacheServices.Domain;
using Core.CryptService;
using Core.DbModels.Base.Di;
using Core.IdentityService;
using Core.Kafka;
using Core.Mapping;
using Core.Repositories;
using Core.SessionServices;

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
        } 
    }
}
