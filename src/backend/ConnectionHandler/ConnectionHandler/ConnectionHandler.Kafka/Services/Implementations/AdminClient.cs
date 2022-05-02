using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Core.Kafka.Domain;
using Core.Kafka.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Core.Kafka.Services.Implementations;

public class AdminClient : IKafkaAdminClient
{
    private readonly IOptions<KafkaOptions> _kafkaOptions;
    
    public AdminClient(IOptions<KafkaOptions> kafkaOptions)
    {
        _kafkaOptions = kafkaOptions;
    }
    
    
    public async Task CreateTopicAsync(string topicName)
    {
        var adminClient = new AdminClientBuilder(new[]
        {
            new KeyValuePair<string, string>("bootstrap.servers", _kafkaOptions.Value.BootstrapServer)
        }).Build();

        try
        {
            await adminClient.CreateTopicsAsync(new[]
            {
                new TopicSpecification()
                {
                    Name = topicName
                }
            });
        }
        catch (Exception ex)
        {
        }
    }
}