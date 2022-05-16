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
    

    public async Task SendMessageToUserAsync(MessageToSendInNetwork messageToSend)
    {
        var subscriber = _databaseProvider.GetSubscribers();
        _messageOffsetService.IncrementNotificationOffset(messageToSend.UserId);
        foreach (var message in messageToSend.Messages)
        {
            var channelName = GetChannelName(messageToSend.UserId, message.ConnectionId);
            subscriber.Publish(new RedisChannel(channelName, RedisChannel.PatternMode.Auto),
                message.Message, CommandFlags.FireAndForget);
        }
    }
    
    private string GetChannelName(Guid userId, string connectionId)
    {
        return $"{userId}-{connectionId}";
    }
}