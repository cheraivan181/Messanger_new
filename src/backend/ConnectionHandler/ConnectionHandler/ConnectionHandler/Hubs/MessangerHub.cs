using ConnectionHandler.Models.Requests;
using ConnectionHandler.Services.Interfaces;
using Core.BinarySerializer;
using Core.Kafka.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace ConnectionHandler.Hubs;

[Authorize(Roles = "ProtocoledUser")]
public class MessangerHub : BaseHub
{
    private readonly IHubCallerContextStore _hubCallerContextStore;
    private readonly IClientConnectionService _clientConnectionService;
    private readonly IRedisBusService _redisBusService;
    private readonly IProducerService _producerService;
    private readonly string TopicName;
    
    public MessangerHub(IHubCallerContextStore hubCallerContextStore,
        IClientConnectionService clientConnectionService,
        IRedisBusService redisBusService,
        IProducerService producerService)
    {
        _hubCallerContextStore = hubCallerContextStore;
        _clientConnectionService = clientConnectionService;
        _redisBusService = redisBusService;
        _producerService = producerService;
        TopicName = "messages";
    }
    
    public async override Task OnConnectedAsync()
    {
        Log.Debug($"{Context.User.Identity.Name} was connected");
        _hubCallerContextStore.AddHubCallerContext(Context.ConnectionId, Context);

        await _clientConnectionService.ConnectAsync(Context.ConnectionId, UserId.ToString(), SessionId);
        await _redisBusService.SubscribeToChanel(UserId, Context.ConnectionId);
        
        await base.OnConnectedAsync();
    }

    [HubMethodName("update")]
    public async Task EventAsync(string message)
    {
        var dispatchMessage = new DispatchMessageRequest()
        {
            SessionId = Guid.Parse(SessionId),
            Message = message,
            UserId = UserId,
            ConnectionId = Context.ConnectionId
        }.ToBinaryMessage();
        
        await _producerService.ProduceAsync(TopicName, dispatchMessage); // partition while partition task 
    }
    
    
    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        _hubCallerContextStore.RemoveCallerContext(Context.ConnectionId);
        await _clientConnectionService.DisconnectAsync(Context.ConnectionId, UserId.ToString(), SessionId);
        await _redisBusService.UnsubscribeChanel(UserId, Context.ConnectionId);
        
        await base.OnDisconnectedAsync(exception);
    }
}