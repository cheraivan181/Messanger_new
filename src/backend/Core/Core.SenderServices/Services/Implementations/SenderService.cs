using Core.CacheServices.Interfaces.Base;
using Core.MessageServices.Services.Interfaces;
using Core.SenderServices.Domain;
using StackExchange.Redis;

namespace Core.MessageServices.Services.Implementations;

public class SenderService : ISenderService
{
    private readonly IDatabaseProvider _databaseProvider;
    private readonly IMessageOffsetService _messageOffsetService;
    
    public SenderService(IDatabaseProvider databaseProvider,
        IMessageOffsetService messageOffsetService)
    {
        _databaseProvider = databaseProvider;
        _messageOffsetService = messageOffsetService;
    }
    

    public async Task SendMessageToUserAsync(MessageToSendInNetwork messageToSend,
        bool isSendMessageToSpecialChannel = false)
    {
        var subscriber = _databaseProvider.GetSubscribers();
        await _messageOffsetService.IncrementNotificationOffsetAsync(messageToSend.UserId);
        foreach (var message in messageToSend.Messages)
        {
            var channelName = GetChannelName(messageToSend.UserId, message.ConnectionId, isSendMessageToSpecialChannel);
            subscriber.Publish(new RedisChannel(channelName, RedisChannel.PatternMode.Auto),
                message.Message, CommandFlags.FireAndForget);
        }
    }
    
    private string GetChannelName(Guid userId, string connectionId, bool isSpecial)
    {
        if (!isSpecial)
            return $"{userId}-{connectionId}";
        return $"{userId}-{connectionId}-special";
    }
}