using Connector;

namespace ConnectionHandler.Services.Interfaces;

public interface IClientConnectionService
{
    Task<ConnectorResponse> ConnectAsync(string connectionId);
    Task<ConnectorResponse> DisconnectAsync(string connectionId);
}