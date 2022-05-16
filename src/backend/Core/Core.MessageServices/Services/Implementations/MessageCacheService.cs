using System.Collections.Concurrent;
using System.Text;
using Core.BinarySerializer;
using Core.CacheServices.Interfaces.Base;
using Core.DbModels;
using Core.MessageServices.Domain;
using Core.MessageServices.Services.Interfaces;
using Core.Utils;
using StackExchange.Redis;

namespace Core.MessageServices.Services.Implementations;

public class MessageCacheService : IMessageCacheService
{
    private readonly IDatabaseProvider _databaseProvider;
    private readonly IMessageMapper _messageMapper;
    
    public MessageCacheService(IDatabaseProvider databaseProvider,
        IMessageMapper messageMapper)
    {
        _databaseProvider = databaseProvider;
        _messageMapper = messageMapper;
    }


    public async Task AddMessageInCacheAsync(Guid dialogId, Message message)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(dialogId);

        if (await database.SetContainsAsync(CommonConstants.MessageCacheInitializerKey, cacheKey))
        {
            return;
        }
        
        //todo:: run lua script
        
        database.KeyExpire(cacheKey, TimeSpan.FromMinutes(CommonConstants.MinutesStoreDialogMessages), 
            CommandFlags.FireAndForget);
        
        var lenght = await database.ListLengthAsync(cacheKey);
        var mapResult = _messageMapper.Map(message).ToBinaryMessage();
        
        if (lenght < CommonConstants.CacheDialogMessages)
        {
            await database.ListRightPushAsync(cacheKey, mapResult);
        }

        await database.ListSetByIndexAsync(cacheKey, CommonConstants.CacheDialogMessages, mapResult);
    }

    public async Task UpdateMessageAsync(Guid dialogId, Message message)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(dialogId);

        var list = await database.ListRangeAsync(cacheKey, 0, -1);
        int index = 0;
        for (int i = 0; i < CommonConstants.CacheDialogMessages; i++)
        {
            if (list[i].ToString().FromBinaryMessage<MessageModel>().MessageId == message.Id)
            {
                index = i;
                break;
            }
        }

        var mapResult = _messageMapper.Map(message).ToBinaryMessage();
        await database.ListSetByIndexAsync(cacheKey, index, mapResult);
    }

    public async Task<List<string>> GetMessagesFromCacheAsync(Guid dialogId)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(dialogId);

        var result = await database.ListRangeAsync(cacheKey, 0, -1);
        return result.Select(x => x.ToString())
            .ToList();
    }

    private string GetCacheKey(Guid dialogId) =>
        $"messagecache-{dialogId}";

    private string GetScript(string cacheKey)
    {
        //TODO:: Implement lua script for redis 
        
        var sb = new StringBuilder();
        sb.AppendLine("require 'rubygems'");
        sb.AppendLine("require 'redis'");
        sb.AppendLine("r = Redis.New");
        sb.AppendLine("");

        return sb.ToString();
    }
}