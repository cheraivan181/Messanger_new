using Core.Kafka.Domain;
using Core.Kafka.Services.Implementations;
using Core.Kafka.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Kafka
{
    public static class DiExtensions
    {
        public static void AddKafkaServices(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.Configure<KafkaOptions>(configuration.GetSection("kafka"));
            serviceCollection.AddSingleton<IProducerSubscriberProvider, ProducerSubscriberSubscriberProvider>();
            serviceCollection.AddSingleton<IKafkaAdminClient, AdminClient>();
            serviceCollection.AddScoped<IProducerService, ProducerService>();
        }
    }
}
