using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Core.Kafka.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Core.HostingServices;

public class ReceiveMessageHostingService : IHostedService
{
    private readonly IProducerSubscriberProvider _producerSubscriberProvider;
    private readonly IServiceProvider _serviceProvider;

    public ReceiveMessageHostingService(IProducerSubscriberProvider producerSubscriberProvider,
        IServiceProvider serviceProvider)
    {
        _producerSubscriberProvider = producerSubscriberProvider;
        _serviceProvider = serviceProvider;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var token = new CancellationTokenSource();
        // Task.Run(async () =>
        // {
        //     using var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        //     var p = scope.ServiceProvider.GetRequiredService<IProducerService>();
        //     int count = 0;
        //     while (!token.IsCancellationRequested)
        //     {
        //         await p.ProduceAsync("s", new {isFisting = true});
        //         count++;
        //     }
        //     Log.Debug($"Publisher sends {count} messages");
        // });
        
        var task = Task.Run( async() =>
        {
            token.Cancel();
            Log.Information($"{nameof(ReceiveMessageHostingService)} was started");
            var consumer = _producerSubscriberProvider.GetConsumer(groupId: "consumer2"); 
            consumer.Subscribe("s"); 
            //consumer.Subscribe("s");
            
            int count = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
              //  var newMessage = newConsumer.Consume(cancellationToken);
                var message = consumer.Consume(cancellationToken);
                
                Log.Information( $"Received message: {message.Value}"); 
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