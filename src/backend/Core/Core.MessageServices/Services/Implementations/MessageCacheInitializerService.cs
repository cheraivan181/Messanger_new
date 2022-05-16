using Core.BinarySerializer;
using Core.CacheServices.Interfaces.Base;
using Core.MessageServices.Services.Interfaces;
using Core.Repositories.Interfaces;
using Core.SessionServices.Services.Interfaces;
using Core.Utils;
using StackExchange.Redis;

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
        var messageLists = await _messageRepository.GetMessageListsAsync(userId);

        if (await database.SetContainsAsync(CommonConstants.MessageCacheInitializerKey, new RedisValue(userId.ToString())))
        {
            return;
        }
        
        var batch = database.CreateBatch();

        foreach (var messageList in messageLists)
        {
            var cacheKey = GetCacheKey(messageList.Key);
            KeysCache.Cache.TryAdd(userId, cacheKey);
            
            batch.KeyExpireAsync(cacheKey.ToRedisKey(), TimeSpan.FromMinutes(CommonConstants.MinutesStoreDialogMessages)); // Нужны наблюдения. Метрики, например 
            
            foreach (var message in messageList.Value)
            {
                var mapMessage = _messageMapper.Map(message).ToBinaryMessage();
                batch.ListRightPushAsync(cacheKey.ToRedisKey(), mapMessage);
            }

            batch.SetAddAsync(CommonConstants.MessageCacheInitializerKey, cacheKey);
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
            batch.SetRemoveAsync(CommonConstants.MessageCacheInitializerKey, cacheKey);
        }
        
        batch.Execute();
    }

    private string GetCacheKey(Guid dialogId) =>
        $"{dialogId}-messageCache";
}