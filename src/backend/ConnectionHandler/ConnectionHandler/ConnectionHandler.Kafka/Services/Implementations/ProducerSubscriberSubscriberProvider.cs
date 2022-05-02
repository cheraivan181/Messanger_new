using System.Net;
using Confluent.Kafka;
using Core.Kafka.Domain;
using Core.Kafka.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Core.Kafka.Services.Implementations;

public class ProducerSubscriberSubscriberProvider : IProducerSubscriberProvider
{
    private readonly IOptions<KafkaOptions> _kafkaOptions;
    
    private static IProducer<Null, string> Producer;
    
    private static Dictionary<string, IConsumer<Ignore, string>> Consumers 
        = new Dictionary<string, IConsumer<Ignore, string>>();

    public ProducerSubscriberSubscriberProvider(IOptions<KafkaOptions> kafkaOptions)
    {
        _kafkaOptions = kafkaOptions;
    }
    
    public IProducer<Null, string> GetProducer()
    {
        if (Producer == null)
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = _kafkaOptions.Value.BootstrapServer,
                ClientId = Dns.GetHostName(),
                CompressionType = CompressionType.Gzip,
                LingerMs = 20,
                BatchSize = 700000
            };
        
            Producer = new ProducerBuilder<Null, string>(config).Build();
        }

        return Producer;
    }

    public IConsumer<Ignore, string> GetConsumer(string groupId = "")
    {
        if (!Consumers.TryGetValue(groupId, out IConsumer<Ignore, string> consumer)
            || consumer == null)
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = _kafkaOptions.Value.BootstrapServer,
                ClientId = Dns.GetHostName()
            };

            if (!string.IsNullOrEmpty(groupId))
                config.GroupId = groupId;
            
            Consumers.Add(groupId, new ConsumerBuilder<Ignore, string>(config).Build());
            return Consumers[groupId];
        }

        return consumer;
    }
}