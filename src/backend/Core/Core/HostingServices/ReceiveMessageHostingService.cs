using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Core.BinarySerializer;
using Core.CryptProtocol.Domain;
using Core.Kafka.Services.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Core.HostingServices;


/// <summary>
/// По сути это будет ядро. Подписка на топик событий и процессинг их 
/// </summary>
public class ReceiveMessageHostingService : IHostedService
{
    private readonly IProducerSubscriberProvider _producerSubscriberProvider;
    private readonly IMessageDispatcherService _messageDispatcherService;
    
    public ReceiveMessageHostingService(IProducerSubscriberProvider producerSubscriberProvider,
        IMessageDispatcherService messageDispatcherService)
    {
        _producerSubscriberProvider = producerSubscriberProvider;
        _messageDispatcherService = messageDispatcherService;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var task = Task.Run( async() =>
        {
            Log.Information($"{nameof(ReceiveMessageHostingService)} was started");
            var consumer = _producerSubscriberProvider.GetConsumer(groupId: "consumer1"); 
            consumer.Subscribe("messages");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = consumer.Consume(cancellationToken);
                var deserializedMessage = message.Message.Value.FromBinaryMessage<DispatchMessageRequest>();
                _messageDispatcherService.DispatchMessage(deserializedMessage);
            }
        }, cancellationToken);
        
        if (task.IsCompleted)
        {
            return task;
        }
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Log.Information($"{nameof(ReceiveMessageHostingService)} is stopping");
        return Task.CompletedTask;
    }
}