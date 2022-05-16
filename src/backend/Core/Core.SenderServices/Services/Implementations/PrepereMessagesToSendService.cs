using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Services.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Core.SenderServices.Domain;
using Core.SessionServices.Services.Interfaces;

namespace Core.MessageServices.Services.Implementations;

public class PrepereMessagesToSendService : IPrepereMessagesToSendService
{
    private readonly IResponseBuilder _responseBuilder;
    private readonly ISessionGetterService _sessionGetterService;
    private readonly IConnectionCollectorService _connectionCollectorService;
    private readonly IMessageOffsetService _messageOffsetService;
    
    public PrepereMessagesToSendService(IResponseBuilder responseBuilder,
        IConnectionCollectorService connectionCollectorService,
        ISessionGetterService sessionGetterService,
        IMessageOffsetService messageOffsetService)
    {
        _responseBuilder = responseBuilder;
        _connectionCollectorService = connectionCollectorService;
        _sessionGetterService = sessionGetterService;
        _messageOffsetService = messageOffsetService;
    }
    
    public async Task<MessageToSendInNetwork> BuildMessageToSendResponseAsync<T>(T responseModel,
        Guid userId, ResponseAction responseAction, ResponseCode code = ResponseCode.Sucess,
        string skippedConnectionId = null) where T:class, ISerializableMessage
    {
        var sessions = await _sessionGetterService.GetSessionsAsync(userId);
        var connections = await _connectionCollectorService.GetConnectionsAsync(userId);
        var notificationOffsets = _messageOffsetService.GetNotificationOffsets(userId);
        
        var result = new MessageToSendInNetwork();
        
        foreach (var connection in connections.Connections.Where(x => x.ConnectionId != skippedConnectionId))
        {
            var currentSession = sessions.Single(x => x.SessionId == connection.SessionId);
            var currentOffset = notificationOffsets.Single(x => x.Key == connection.ConnectionId);
            
            var message = _responseBuilder.BuildMessage<T>(responseModel, code, responseAction, currentSession.AesKey,
                currentSession.HmacKey, currentOffset.Value);
            
            result.Messages.Add(new MessageToSend(connection.ConnectionId, message));
        }

        return result;
    }

    public async Task<MessageToSendInNetwork> BuildMessageForSingleConnection<T>(T responseModel, Guid userId,
        string connectionId, ResponseAction responseAction, ResponseCode responseCode) where T:class, ISerializableMessage
    {
        var result = new MessageToSendInNetwork();
        var connections = await _connectionCollectorService.GetConnectionsAsync(userId);
        var notificationsOffset =  _messageOffsetService.GetNotificationOffsets(userId);
        var currentNotificationOffset = notificationsOffset.Single(x => x.Key == connectionId);

        var currentConnection = connections.Connections.Single(x => x.ConnectionId == connectionId);
        var session = await _sessionGetterService.GetSessionAsync(userId, currentConnection.SessionId);
        
        var messsage = _responseBuilder.BuildMessage<T>(responseModel, responseCode, responseAction,
            session.AesKey, session.HmacKey, currentNotificationOffset.Value);
        
        result.Messages.Add(new MessageToSend(connectionId, messsage));
        return result;
    }
}