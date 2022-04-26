using Connector;
using Core.MessageServices.Services.Interfaces;
using Core.SessionServices.Services.Interfaces;
using Grpc.Core;

namespace Core.GRPC;

public class ConnectionGrpcService : Connector.Connector.ConnectorBase
{
    private readonly ISessionCacheInitializerService _sessionCacheInitializerService;
    private readonly ISessionCacheService _sessionCacheService;
    private readonly IMessageOffsetService _messageOffsetService;
    private readonly IMessageCacheInitializerService _messageCacheInitializerService;
    private readonly IConnectionCollectorService _connectionCollectorService;

    public ConnectionGrpcService(ISessionCacheInitializerService sessionCacheInitializerService,
        ISessionCacheService sessionCacheService,
        IMessageOffsetService offsetService,
        IMessageCacheInitializerService messageCacheInitializerService,
        IConnectionCollectorService connectionCollectorService)
    {
        _sessionCacheInitializerService = sessionCacheInitializerService;
        _messageOffsetService = offsetService;
        _sessionCacheService = sessionCacheService;
        _messageCacheInitializerService = messageCacheInitializerService;
        _connectionCollectorService = connectionCollectorService;
    }
    
    public async override Task<ConnectorResponse> ConnectUser(UserConnectRequest request, ServerCallContext serverCallContext)
    {
        var parsedUserId = Guid.Parse(request.UserId);
        var parsedSessionId = Guid.Parse(request.SessionId);
        
        await _sessionCacheInitializerService.SetSessionInCacheAsync(parsedSessionId, parsedUserId);
        
        await _messageOffsetService.RegisterOffsetAsync(parsedUserId, request.ConnectionId);
        await _messageCacheInitializerService.InitializeMessageCacheAsync(parsedUserId);
        await _connectionCollectorService.AddConnectionAsync(parsedUserId, parsedSessionId, request.ConnectionId);
        
        var connectorResponse = new ConnectorResponse()
        {
            SucessHandled = true
        };

        return connectorResponse;
    }
    
    
    public async override Task<ConnectorResponse> DisconnectUser(UserDisconnectRequest request, ServerCallContext serverCallContext)
    {
        var parsedUserId = Guid.Parse(request.UserId);
        
        await _sessionCacheService.RemoveSessonFromCacheAsync(parsedUserId, Guid.Parse(request.SessionId));
        await _messageOffsetService.RemoveConnectionFromNotificationOffsetAsync(parsedUserId, request.ConnectionId);
        await _messageCacheInitializerService.RemoveMessagesFromCacheAsync(parsedUserId);
        await _connectionCollectorService.RemoveConnectionAsync(parsedUserId, request.ConnectionId);
        
        var connectorResponse = new ConnectorResponse()
        {
            SucessHandled = true
        };

        return connectorResponse;
    }
}