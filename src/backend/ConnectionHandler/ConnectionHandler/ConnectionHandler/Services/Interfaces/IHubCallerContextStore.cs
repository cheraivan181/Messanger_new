using Microsoft.AspNetCore.SignalR;

namespace ConnectionHandler.Services.Interfaces;

public interface IHubCallerContextStore
{
    void AddHubCallerContext(string connectionId, HubCallerContext context);
    HubCallerContext GetCallerContext(string connectionId);
    void RemoveCallerContext(string connectionId);
}