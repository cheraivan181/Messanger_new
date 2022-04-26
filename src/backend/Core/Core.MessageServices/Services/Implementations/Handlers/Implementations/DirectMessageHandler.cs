using Core.CryptProtocol.Domain;
using Core.CryptProtocol.Services.Interfaces;
using Core.MessageServices.Services.Implementations.Handlers.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Core.Repositories.Interfaces;

namespace Core.MessageServices.Services.Implementations.Handlers.Implementations;

public class DirectMessageHandler : IDirectMessageHandler
{
    private readonly ISenderService _senderService;
    private readonly IMessageCacheService _messageCacheService;
    private readonly IPrepereMessagesToSendService _prepereMessageToSendService;
    
    private readonly IMessageRepository _messageRepository;

    public DirectMessageHandler(IResponseBuilder responseBuilder,
        ISenderService senderService,
        IMessageCacheService messageCacheService,
        IPrepereMessagesToSendService prepereMessageToSendService)
    {
        _senderService = senderService;
        _messageCacheService = messageCacheService;
        _prepereMessageToSendService = prepereMessageToSendService;
    }
    
    public async Task HandleAsync(SendDirectMessageRequest request)
    {
        
    }
}