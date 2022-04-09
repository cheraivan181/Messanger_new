using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace ConnectionHandler.Hubs;

[Authorize(Roles = "ProtocoledUser")]
public class MessangerHub : BaseHub
{
    public async override Task OnConnectedAsync()
    {
        Log.Debug($"{Context.User.Identity.Name} was connected");
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
        return base.OnDisconnectedAsync(exception);
    }
}