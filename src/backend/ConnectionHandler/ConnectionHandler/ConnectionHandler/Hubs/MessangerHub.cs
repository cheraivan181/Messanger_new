using ConnectionHandler.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace ConnectionHandler.Hubs;

[Authorize(Roles = "ProtocoledUser")]
public class MessangerHub : BaseHub
{
    private readonly IHubCallerContextStore _hubCallerContextStore;

    public MessangerHub(IHubCallerContextStore hubCallerContextStore)
    {
        _hubCallerContextStore = hubCallerContextStore;
    }
    
    public async override Task OnConnectedAsync()
    {
        Log.Debug($"{Context.User.Identity.Name} was connected");
        _hubCallerContextStore.AddHubCallerContext(Context.ConnectionId, Context);
        await base.OnConnectedAsync();
    }
    
    [HubMethodName("SendMessage")]
    public async Task SendMessageAsync()
    {
    }
    
    
    [HubMethodName("ReadMessage")]
    public async Task ReadMessageAsync()
    {
    }
    
    [HubMethodName("subscribeToMessageStream")]
    public async IAsyncEnumerable<string> SubscribeToMessageTopic()
    {
        yield break;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _hubCallerContextStore.RemoveCallerContext(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}