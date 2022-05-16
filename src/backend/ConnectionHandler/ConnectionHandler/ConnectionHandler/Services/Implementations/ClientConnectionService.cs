using ConnectionHandler.Options;
using ConnectionHandler.Services.Interfaces;
using Connector;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace ConnectionHandler.Services.Implementations;

public class ClientConnectionService : IClientConnectionService
{
    private readonly IOptions<UrlOptions> _urlOptions;
    private GrpcChannel _channel;
    private SocketsHttpHandler handler = new SocketsHttpHandler()
    {
        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
        KeepAlivePingDelay = TimeSpan.FromSeconds(60),
        KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
        EnableMultipleHttp2Connections = true
    };


    public ClientConnectionService(IOptions<UrlOptions> urlOptions)
    {
        _urlOptions = urlOptions;
    }

    public async Task<ConnectorResponse> ConnectAsync(string connectionId, string userId, string sessionId)
    {
        var channel = GetChannel();
        Connector.Connector.ConnectorClient client = new(channel);

        var userConnectRequest = new UserConnectRequest()
        {
            ConnectionId = connectionId,
            UserId = userId,
            SessionId = sessionId
        };
        
        var response = await client.ConnectUserAsync(userConnectRequest);
        return response;
    }

    public async Task<ConnectorResponse> DisconnectAsync(string connectionId, string userId, string sessionId)
    {
        var channel = GetChannel();
        Connector.Connector.ConnectorClient client = new(channel);

        var userConnectRequest = new UserConnectRequest()
        {
            ConnectionId = connectionId,
            UserId = userId,
            SessionId = sessionId
        };
        
        var response = await client.ConnectUserAsync(userConnectRequest);
        return response;
    }

    private GrpcChannel GetChannel()
    {
        if (_channel == null)
        {
            _channel = GrpcChannel.ForAddress(_urlOptions.Value.CoreUrl, new GrpcChannelOptions()
            {
                HttpHandler = handler
            });
        }

        return _channel;
    }
}