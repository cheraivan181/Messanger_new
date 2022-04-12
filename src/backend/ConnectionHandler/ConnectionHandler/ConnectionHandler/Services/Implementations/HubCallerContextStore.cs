using ConnectionHandler.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ConnectionHandler.Services.Implementations;

public class HubCallerContextStore : IHubCallerContextStore
{
    private static Dictionary<string, HubCallerContext> _hubCallerContextStore = new Dictionary<string, HubCallerContext>();

    public void AddHubCallerContext(string connectionId, HubCallerContext context)
    {
        _hubCallerContextStore[connectionId] = context;
    }

    public HubCallerContext GetCallerContext(string connectionId) =>
        _hubCallerContextStore[connectionId];

    public void RemoveCallerContext(string connectionId)
    {
        if (_hubCallerContextStore.ContainsKey(connectionId))
            _hubCallerContextStore.Remove(connectionId);
    }
}