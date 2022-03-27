using Confluent.Kafka;

namespace Core.Kafka.Services.Interfaces;

public interface IProducerSubscriberProvider
{
    IProducer<Null, string> GetProducer();
    IConsumer<Ignore, string> GetConsumer(string groupId = "");
}