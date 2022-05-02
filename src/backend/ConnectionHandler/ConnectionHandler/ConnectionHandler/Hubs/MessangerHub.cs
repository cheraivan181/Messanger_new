using ConnectionHandler.Models.Requests;
using ConnectionHandler.Services.Interfaces;
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
    private readonly IConnectionChanelsStorage _connectionChanelsStorage;
    private readonly IRedisBusService _redisBusService;
    private readonly IProducerService _producerService;
    
    public MessangerHub(IHubCallerContextStore hubCallerContextStore,
        IClientConnectionService clientConnectionService,
        IConnectionChanelsStorage connectionChanelsStorage,
        IRedisBusService redisBusService,
        IProducerService producerService)
    {
        _hubCallerContextStore = hubCallerContextStore;
        _clientConnectionService = clientConnectionService;
        _connectionChanelsStorage = connectionChanelsStorage;
        _redisBusService = redisBusService;
        _producerService = producerService;
    }
    
    public async override Task OnConnectedAsync()
    {
        Log.Debug($"{Context.User.Identity.Name} was connected");
        _hubCallerContextStore.AddHubCallerContext(Context.ConnectionId, Context);
        _connectionChanelsStorage.AddChannel(Context.ConnectionId);

        await _clientConnectionService.ConnectAsync(Context.ConnectionId);
        await _redisBusService.SubscribeToChanel(UserId, Context.ConnectionId);
        
        await base.OnConnectedAsync();
    }
    
    [HubMethodName("sendMessage")]
    public async Task SendMessageAsync(RequestMessage requestMessage)
    {
    }

    [HubMethodName("readMessage")]
    public async Task ReadMessageAsync()
    {
    }
    
    [HubMethodName("getDialogMessagesAsync")]
    public async Task GetDialogMessagesAsync()
    {
    }

    
    [HubMethodName("sendDirectMessage")]
    public async Task OnSendDirectMessageAsync()
    {
    }
    
    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        _hubCallerContextStore.RemoveCallerContext(Context.ConnectionId);
        
        await _clientConnectionService.DisconnectAsync(Context.ConnectionId);
        _connectionChanelsStorage.RemoveChannel(Context.ConnectionId);
        
        await base.OnDisconnectedAsync(exception);
    }
}