using Core.BinarySerializer;

namespace Core.Kafka.Services.Interfaces;

public interface IProducerService
{
    Task<bool> ProduceAsync(string topicName, string message, int partition = 0);
}