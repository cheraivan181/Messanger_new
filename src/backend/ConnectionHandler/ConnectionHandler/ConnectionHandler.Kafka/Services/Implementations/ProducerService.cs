using System.Net;
using Confluent.Kafka;
using Core.BinarySerializer;
using Core.Kafka.Domain;
using Core.Kafka.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Core.Kafka.Services.Implementations
{
    public class ProducerService : IProducerService
    {
        private readonly IOptions<KafkaOptions> _kafkaOptions;
        
        public ProducerService(IOptions<KafkaOptions> kafkaOptions)
        {
            _kafkaOptions = kafkaOptions;
        }

        public async Task<bool> ProduceAsync<T>(string topicName, T objectToProduct, int partition = 0) where T:class, ISerializableMessage
        {
            var msgId = DateTime.Now.Ticks;
            var message = objectToProduct.ToBinaryMessage();
            
            var config = new ProducerConfig()
            {
                BootstrapServers = _kafkaOptions.Value.BootstrapServer,
                ClientId = Dns.GetHostName()
            }; 
            
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var result = await producer.ProduceAsync(new TopicPartition(topicName, new Partition(partition)), new Message<Null, string>()
                {
                    Value = message
                }, CancellationToken.None);

                if (result.Status == PersistenceStatus.NotPersisted)
                {
                    return false;
                }
                
                return true;
            }
        }
    }
}
