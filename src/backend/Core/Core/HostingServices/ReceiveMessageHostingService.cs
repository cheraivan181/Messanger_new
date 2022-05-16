using Core.BinarySerializer;
using Core.CryptProtocol.Domain;
using Core.Kafka.Services.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Core.Utils;
using Serilog;

namespace Core.HostingServices;

/// <summary>
/// По сути это будет ядро. Подписка на топик событий и процессинг их 
/// </summary>
public class ReceiveMessageHostingService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    
    public ReceiveMessageHostingService(IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var task = Task.Run( async() =>
        {
            using var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var producerSubscriberProvider = scope.ServiceProvider.GetRequiredService<IProducerSubscriberProvider>();
            var messageDispatcherService = scope.ServiceProvider.GetRequiredService<IMessageRouterService>();
             
            Log.Information($"{nameof(ReceiveMessageHostingService)} was started");
            
            var consumer = producerSubscriberProvider.GetConsumer(groupId: "consumer1"); //TODO:: while sharding task
            consumer.Subscribe(CommonConstants.MessagesTopic); 
            
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = consumer.Consume(cancellationToken);
                var deserializedMessage = message.Message.Value.FromBinaryMessage<DispatchMessageRequest>();
                messageDispatcherService.DispatchMessageAsync(deserializedMessage);
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