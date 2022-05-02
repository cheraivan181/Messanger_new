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

        var list = await database.ListRangeAsync(cacheKey, 0, CommonConstants.CacheDialogMessages);
        int index = 0;
        for (int i = 0; i < CommonConstants.CacheDialogMessages; i++)
            if (list[i].ToString().FromBinaryMessage<MessageModel>().MessageId == message.Id)
                index = i;

        var mapResult = _messageMapper.Map(message).ToBinaryMessage();
        await database.ListSetByIndexAsync(cacheKey, index, mapResult);
    }

    public async Task<List<string>> GetMessagesFromCacheAsync(Guid dialogId)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(dialogId);

        var result = await database.ListRangeAsync(cacheKey, 0, CommonConstants.CacheDialogMessages);
        return result.Select(x => x.ToString())
            .ToList();
    }

    private string GetCacheKey(Guid dialogId) =>
        $"messagecache-{dialogId}";

    private string GetScript(string cacheKey)
    {
        var sb = new StringBuilder();
        sb.AppendLine("require 'rubygems'");
        sb.AppendLine("require 'redis'");
        sb.AppendLine("r = Redis.New");
        sb.AppendLine("");

        return sb.ToString();
    }
}