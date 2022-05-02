using Core.BinarySerializer;
using Core.CacheServices.Interfaces.Base;
using Core.MessageServices.Services.Interfaces;
using Core.Repositories.Interfaces;
using Core.SessionServices.Services.Interfaces;
using Core.Utils;

namespace Core.MessageServices.Services.Implementations;

public class MessageCacheInitializerService : IMessageCacheInitializerService
{
    private readonly IDatabaseProvider _databaseProvider;
    private readonly IMessageRepository _messageRepository;
    private readonly IDialogRepository _dialogRepository;
    private readonly IMessageMapper _messageMapper;
    private readonly IConnectionCollectorService _connectionCollectorService;
    
    public MessageCacheInitializerService(IMessageRepository messageRepository,
        IDialogRepository dialogRepository,
        IDatabaseProvider databaseProvider,
        IMessageMapper messageMapper,
        IConnectionCollectorService connectionCollectorService)
    {
        _dialogRepository = dialogRepository;
        _messageRepository = messageRepository;
        _databaseProvider = databaseProvider;
        _messageMapper = messageMapper;
        _connectionCollectorService = connectionCollectorService;
    }
    
    public async Task InitializeMessageCacheAsync(Guid userId)
    {
        var database = _databaseProvider.GetDatabase();
        var initializeCacheKey = GetInitializedCacheKey(userId);
        
        if (await database.KeyExistsAsync(initializeCacheKey))
            return;
        
        var messageLists = await _messageRepository.GetMessageListsAsync(userId);
        
        var batch = database.CreateBatch();
           
        foreach (var messageList in messageLists)
        {
            var cacheKey = GetCacheKey(messageList.Key);
            foreach (var message in messageList.Value)
            {
                var mapMessage = _messageMapper.Map(message).ToBinaryMessage();
                batch.ListRightPushAsync(cacheKey.ToRedisKey(), mapMessage);
            }
        }
        
        batch.Execute();
    }

    public async Task RemoveMessagesFromCacheAsync(Guid userId)
    {
        var connections = await _connectionCollectorService.GetConnectionsAsync(userId);
        if (connections == null || connections.Connections.Count == 0)
            return;

        var userDialogs = await _dialogRepository.GetUserDialogsAsync(userId);
        var database = _databaseProvider.GetDatabase();
        var batch = database.CreateBatch();
        
        foreach (var dialog in userDialogs)
        {
            var interlocutorId = dialog.User1Id == userId
                ? dialog.User2Id
                : dialog.User1Id;

            var interlocutorConnections = await _connectionCollectorService.GetConnectionsAsync(interlocutorId);
            if (interlocutorConnections != null || interlocutorConnections.Connections.Count > 0)
                continue;

            var cacheKey = GetCacheKey(dialog.Id);
            batch.KeyDeleteAsync(cacheKey);
        }
        
        batch.Execute();
    }

    private string GetCacheKey(Guid dialogId) =>
        $"{dialogId}-messageCache";

    private string GetInitializedCacheKey(Guid userId) =>
        $"{userId}-messageCacheInitialized";
}