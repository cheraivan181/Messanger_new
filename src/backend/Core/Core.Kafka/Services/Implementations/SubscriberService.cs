using System.Net;
using Confluent.Kafka;
using Core.Kafka.Domain;
using Core.Kafka.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Core.Kafka.Services.Implementations;

public class SubscriberService
{
    private readonly IProducerSubscriberProvider _producerSubscriberProvider;
    private readonly IOptions<KafkaOptions> _kafkaOptions;

    public SubscriberService(IOptions<KafkaOptions> kafkaOptions,
        IProducerSubscriberProvider producerSubscriberProvider)
    {
        _kafkaOptions = kafkaOptions;
        _producerSubscriberProvider = producerSubscriberProvider;
    }

    public void SubscribeToTopic(string topic, Action action, string groupId = "")
    {
        var consumer = _producerSubscriberProvider.GetConsumer(groupId);
        consumer.Subscribe(topic);
        
        
    }
}