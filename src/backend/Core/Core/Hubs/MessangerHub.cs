using Microsoft.AspNetCore.SignalR;

namespace Core.Hubs
{
    public class MessangerHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        [HubMethodName("SendMessage")]
        public async Task OnSendMessageAsync()
        {

        }

        [HubMethodName("ReadMessage")]
        public async Task OnReadMessageAsync()
        {

        }

        [HubMethodName("OnWriteMessageAsync")]
        public async Task OnWriteMessageAsync()
        {

        }

        [HubMethodName("getmessages")]
        public async IAsyncEnumerable<string> GetMessagesAsync()
        {
            yield break;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
