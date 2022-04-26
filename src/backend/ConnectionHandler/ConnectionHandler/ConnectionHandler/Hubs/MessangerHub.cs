using ConnectionHandler.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace ConnectionHandler.Hubs;

[Authorize(Roles = "ProtocoledUser")]
public class MessangerHub : BaseHub
{
    private readonly IHubCallerContextStore _hubCallerContextStore;
    private readonly IClientConnectionService _clientConnectionService;
    
    public MessangerHub(IHubCallerContextStore hubCallerContextStore,
        IClientConnectionService clientConnectionService)
    {
        _hubCallerContextStore = hubCallerContextStore;
        _clientConnectionService = clientConnectionService;
    }
    
    public async override Task OnConnectedAsync()
    {
        Log.Debug($"{Context.User.Identity.Name} was connected");
        _hubCallerContextStore.AddHubCallerContext(Context.ConnectionId, Context);
        await _clientConnectionService.ConnectAsync(Context.ConnectionId);
        
        await base.OnConnectedAsync();
    }
    
    [HubMethodName("sendMessage")]
    public async Task SendMessageAsync()
    {
    }

    [HubMethodName("readMessage")]
    public async Task ReadMessageAsync()
    {
    }
    
   
    
    [HubMethodName("subscribeToMessageStream")]
    public async IAsyncEnumerable<string> SubscribeToMessageTopic()
    {
        yield break;
    }

    [HubMethodName("getDialogMessagesAsync")]
    public async IAsyncEnumerable<string> GetDialogMessagesAsync()
    {
        yield break;
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        _hubCallerContextStore.RemoveCallerContext(Context.ConnectionId);
        
        await _clientConnectionService.DisconnectAsync(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}