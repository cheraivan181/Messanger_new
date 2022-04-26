using Core.MessageServices.Services.Interfaces;
using Core.Repositories.Interfaces;

namespace Core.MessageServices.Services.Implementations;

public class MessageCacheInitializerService : IMessageCacheInitializerService
{
    private readonly IMessageRepository _messageRepository;
    
    
    public MessageCacheInitializerService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    public Task InitializeMessageCacheAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveMessagesFromCacheAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}