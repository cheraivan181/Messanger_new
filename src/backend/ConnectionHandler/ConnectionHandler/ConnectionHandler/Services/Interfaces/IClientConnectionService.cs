using Connector;

namespace ConnectionHandler.Services.Interfaces;

public interface IClientConnectionService
{
    Task<ConnectorResponse> ConnectAsync(string connectionId, string userId, string sessionId);
    Task<ConnectorResponse> DisconnectAsync(string connectionId, string userId, string sessionId);
}