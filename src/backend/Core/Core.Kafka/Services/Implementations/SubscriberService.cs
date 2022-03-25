using System.Net;
using Confluent.Kafka;
using Core.Kafka.Domain;
using Microsoft.Extensions.Options;

namespace Core.Kafka.Services.Implementations;

public class SubscriberService
{
    private readonly IOptions<KafkaOptions> _kafkaOptions;

    public SubscriberService(IOptions<KafkaOptions> kafkaOptions)
    {
        _kafkaOptions = kafkaOptions;
    }

    public void SubscribeToTopic(string topic, Action action, string groupId = "")
    {
        var config = new ConsumerConfig()
        {
            BootstrapServers = _kafkaOptions.Value.BootstrapServer,
            ClientId = Dns.GetHostName()
        };

        if (!string.IsNullOrEmpty(groupId))
            config.GroupId = groupId;
        
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);
    }
}