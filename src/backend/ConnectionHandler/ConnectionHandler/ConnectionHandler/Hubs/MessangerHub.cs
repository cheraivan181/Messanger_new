using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace ConnectionHandler.Hubs;

public class MessangerHub : BaseHub
{
    [Authorize(Roles = "ProtocoledUser")]
    public async override Task OnConnectedAsync()
    {
        Log.Debug($"{Context.User.Identity.Name} was connected");
        await base.OnConnectedAsync();
    }
    
    [Authorize(Roles = "ProtocoledUser")]
    [HubMethodName("SendMessage")]
    public async Task SendMessageAsync()
    {
    }
    
    
    [Authorize(Roles = "ProtocoledUser")]
    [HubMethodName("ReadMessage")]
    public async Task ReadMessageAsync()
    {
    }
    
    [Authorize(Roles = "ProtocoledUser")]
    [HubMethodName("getmessages")]
    public async IAsyncEnumerable<string> SubscribeToMessageTopic()
    {
        yield break;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}