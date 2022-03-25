using System.Net;
using System.Text.Json;
using Confluent.Kafka;
using Core.Kafka.Domain;
using Core.Kafka.Services.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;

namespace Core.Kafka.Services.Implementations
{
    public class ProducerService : IProducerService
    {
        private readonly IOptions<KafkaOptions> _kafkaOptions;
        
        public ProducerService(IOptions<KafkaOptions> kafkaOptions)
        {
            _kafkaOptions = kafkaOptions;
        }

        public async Task<bool> ProduceAsync(string topicName, object objectToProduct)
        {
            var msgId = DateTime.Now.Ticks;
            var message = JsonSerializer.Serialize(objectToProduct);
            
            Log.Debug($"Publish message in kafka. MsgId: {msgId}," +
                      $" TopicName: {topicName}, Message: {message}");
            
            var config = new ProducerConfig()
            {
                BootstrapServers = _kafkaOptions.Value.BootstrapServer,
                ClientId = Dns.GetHostName()
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var result = await producer.ProduceAsync(topicName, new Message<Null, string>()
                {
                    Value = message
                }, CancellationToken.None);

                if (result.Status == PersistenceStatus.NotPersisted)
                {
                    Log.Error($"Cannot produce message, messageId: {msgId}");
                    return false;
                }

                return true;
            }
        }
    }
}
