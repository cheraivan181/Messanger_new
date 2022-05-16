using ConnectionHandler.Hubs;
using ConnectionHandler.Redis.Interfaces.Base;
using ConnectionHandler.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace ConnectionHandler.Services.Implementations;

public class RedisBusService : IRedisBusService
{
    private readonly IDatabaseProvider _databaseProvider;
    private readonly IHubContext<MessangerHub> _hub;

    public RedisBusService(IDatabaseProvider databaseProvider,
        IHubContext<MessangerHub> hub)
    {
        _databaseProvider = databaseProvider;
        _hub = hub;
    }

    public async Task SubscribeToChanel(Guid userId, string connectionId)
    {
        var subscriber = _databaseProvider.GetSubscribers();
        var subscriberName = GetChanelName(userId, connectionId);
            
        await subscriber.SubscribeAsync(new RedisChannel(GetChanelName(userId, connectionId), RedisChannel.PatternMode.Auto), async (channel, message) =>
        {
            var connectionId = channel.ToString().Split('-')[1];
            await _hub.Clients.Client(connectionId).SendAsync("getupdates",message);
        });
    }

    public async Task UnsubscribeChanel(Guid userId, string channel)
    {
        var subscriber = _databaseProvider.GetSubscribers();
        var channelName = GetChanelName(userId, channel);

        await subscriber.UnsubscribeAsync(channelName);
    }
    
    private string GetChanelName(Guid userId, string connectionId) =>
        $"{userId}-{connectionId}";
}