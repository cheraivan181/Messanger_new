namespace Core.Kafka.Services.Interfaces;

public interface IKafkaAdminClient
{
    Task CreateTopicAsync(string topicName);
}