using Core.BinarySerializer;

namespace Core.Kafka.Services.Interfaces;

public interface IProducerService
{
    Task<bool> ProduceAsync<T>(string topicName, T objectToProduct, int partition = 0)
        where T : class, ISerializableMessage;
}