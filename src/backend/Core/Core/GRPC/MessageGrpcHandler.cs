using Core.MessageServices.Services.Implementations;
using Core.MessageServices.Services.Interfaces;

namespace Core.GRPC;

public class MessageGrpcHandler
{
    private readonly IMessageDispatcherService _messageDispatcherService;

    public MessageGrpcHandler(IMessageDispatcherService messageDispatcherService)
    {
        _messageDispatcherService = messageDispatcherService;
    }
    
    public async Task SendMessageAsync()
    {
        
    }
}