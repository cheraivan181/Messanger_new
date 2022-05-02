using Core.BinarySerializer;
using Core.DbModels;
using Core.MessageServices.Domain;
using Core.MessageServices.Services.Interfaces;
using Core.Repositories.Interfaces;
using Core.Utils;

namespace Core.MessageServices.Services.Implementations;

public class MessageGetterService : IMessageGetterService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMessageCacheService _messageCacheService;
    private readonly IMessageMapper _messageMapper;
    
    public MessageGetterService(IMessageRepository messageRepository,
        IMessageCacheService messageCacheService,
        IMessageMapper messageMapper)
    {
        _messageRepository = messageRepository;
        _messageCacheService = messageCacheService;
        _messageMapper = messageMapper;
    }

    public async IAsyncEnumerable<MessageModels> GetMessageListAsync(Guid dialogId, int page = 0)
    {
        var messageModels = new MessageModels();
        var messagesFromCacheIds = new List<Guid>();
        var messagesFromDatabase = new List<Message>();
        
        if (page == 0)
        {
            var messagesFromCache = await _messageCacheService.GetMessagesFromCacheAsync(dialogId);
            messagesFromCache.ForEach((item) => messageModels.Messages.Add(item.FromBinaryMessage<MessageModel>()));
            messagesFromCacheIds = messageModels.Messages.Select(x => x.MessageId)
                .ToList();

            yield return messageModels;
        }
    
        messagesFromDatabase = await _messageRepository.GetMessageFromDialogAsync(dialogId, page);
        if (messagesFromDatabase.Count < CommonConstants.CountMessagesInPage)
            messageModels.IsLastPage = true;
        
        if (messagesFromCacheIds.Count > 0)
        {
            messagesFromDatabase = messagesFromDatabase.Where(x => !messagesFromCacheIds.Contains(x.Id))
                .ToList();
        }
        
        messageModels.Messages = new List<MessageModel>();
        foreach (var message in messagesFromDatabase)
            messageModels.Messages.Add(_messageMapper.Map(message));

        yield return messageModels;
    }
}