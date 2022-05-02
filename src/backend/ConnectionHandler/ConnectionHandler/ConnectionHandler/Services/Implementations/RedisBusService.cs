using ConnectionHandler.Hubs;
using ConnectionHandler.Redis.Interfaces.Base;
using ConnectionHandler.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace ConnectionHandler.Services.Implementations;

public class RedisBusService : IRedisBusService
{
    private readonly IDatabaseProvider _databaseProvider;
    private readonly IConnectionChanelsStorage _connectionChanelsStorage;
    private readonly IHubContext<MessangerHub> _hub;

    public RedisBusService(IDatabaseProvider databaseProvider,
        IConnectionChanelsStorage connectionChanelsStorage,
        IHubContext<MessangerHub> hub)
    {
        _databaseProvider = databaseProvider;
        _connectionChanelsStorage = connectionChanelsStorage;
        _hub = hub;
    }

    public async Task SubscribeToChanel(Guid userId, string connectionId)
    {
        var subscriber = _databaseProvider.GetSubscribers();
        var subscriberName = GetChanelName(userId, connectionId);
        
        await subscriber.SubscribeAsync(new RedisChannel(GetChanelName(userId, connectionId), RedisChannel.PatternMode.Auto), async (channel, message) =>
        {
            await _hub.Clients.Client(connectionId).SendAsync("getupdates",message);
        });
    }

    private string GetChanelName(Guid userId, string connectionId) =>
        $"{userId}-{connectionId}";
}