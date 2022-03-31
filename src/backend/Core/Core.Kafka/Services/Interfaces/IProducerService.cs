namespace Core.Kafka.Services.Interfaces;

public interface IProducerService
{
    Task<bool> ProduceAsync(string topicName, object objectToProduct, int partition = 0);
}