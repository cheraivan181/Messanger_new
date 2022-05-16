using Core.BinarySerializer;
using Core.CryptProtocol.Domain;
using Core.CryptProtocol.Domain.Base;
using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Domain.Responses;
using Core.CryptProtocol.Services.Interfaces;
using Core.Kafka.Services.Interfaces;
using Core.MessageServices.Services.Implementations.Handlers.Interfaces;
using Core.MessageServices.Services.Implementations.Senders.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Core.SessionServices.Services.Interfaces;
using Core.Utils;
using Kafka.Contract;
using Serilog;

namespace Core.MessageServices.Services.Implementations;

public class MessageRouterService : IMessageRouterService
{
    private readonly IEnumerable<IHandler> _handlers;
    private readonly IEnumerable<ISender> _senders;
    
    private readonly ISessionGetterService _sessionGetterService;
    private readonly IRequestParser _requestParser;

    private readonly IMessageOffsetService _messageOffsetService;
    private readonly IProducerService _producerService;
    
    public MessageRouterService(IEnumerable<IHandler> handlers,
        IEnumerable<ISender> senders,
        ISessionGetterService sessionGetterService,
        IRequestParser requestParser,
        IMessageOffsetService messageOffsetService,
        IProducerService producerService)
    {
        _handlers = handlers;
        _senders = senders;
        _sessionGetterService = sessionGetterService;
        _requestParser = requestParser;
        _messageOffsetService = messageOffsetService;
        _producerService = producerService;
    }
    
    public async Task DispatchMessageAsync(DispatchMessageRequest request)
    {
        var session = await _sessionGetterService.GetSessionAsync(request.UserId, request.SessionId);
        var parsedBaseMessage = _requestParser.ParseMessage(session.HmacKey, request.Message);

        var errorMessageHandler = _senders.Single(x => x.ResponseAction == ResponseAction.Error);
        
        if (!parsedBaseMessage.IsSucess)
        {
            var errorResponse = new ErrorResponse()
            {
                Message = "Cannot parse model",
                ResponseCode = ResponseCode.IncorrectSign,
                IsErrorVisible = false
            };
            
            await errorMessageHandler.SendAsync(request.UserId, errorResponse, request.ConnectionId);
        }

        var parsedMessageModel =
            _requestParser.ParseRequest(session.HmacKey, session.AesKey, parsedBaseMessage.RequestProtocolMessage.Payload);
        
        if (!parsedMessageModel.IsSucess)
        {
            var errorResponse = new ErrorResponse()
            {
                Message = "Cannot parse model",
                ResponseCode = ResponseCode.IncorrectCryptedText,
                IsErrorVisible = false
            };
            
            await errorMessageHandler.SendAsync(request.UserId, errorResponse, request.ConnectionId); 
        }

        int notificationOffset;
        try
        {
            notificationOffset = _messageOffsetService.GetNotificationOffset(request.UserId, request.ConnectionId);
        }
        catch (Exception ex)
        {
            var disconnectMessage = new DisconnectMessage()
            {
                SessionId = request.SessionId,
                UserId = request.UserId,
                ConnectionId = request.ConnectionId
            };
            // publish reconnect message

            await _producerService.ProduceAsync(CommonConstants.DisconnectMessagesTopid, disconnectMessage);
            
            Log.Error("Cannot get notification offset");
            return;
        }

        if (parsedBaseMessage.RequestProtocolMessage.NotificationOffset != notificationOffset)
        {
            //publish reconnect message
        }
        
        switch (parsedBaseMessage.RequestProtocolMessage.RequestType)
        {
            case RequestType.DirectMessage:
                var sendDirectMessageRequest = parsedMessageModel.DecryptedText.FromBinaryMessage<SendDirectMessageRequest>();
                await _handlers.Single(x => x.RequestType == RequestType.DirectMessage).HandleAsync(sendDirectMessageRequest);
                
                break;
            case RequestType.GetMessages:
                var getMessageRequest = parsedMessageModel.DecryptedText.FromBinaryMessage<GetMessageRequest>();
                await _handlers.Single(x => x.RequestType == RequestType.GetMessages).HandleAsync(getMessageRequest);
                
                break;
            case RequestType.UpdateMessage:
                var updateMessageRequest = parsedMessageModel.DecryptedText.FromBinaryMessage<UpdateMessageResquest>();
                await _handlers.Single(x => x.RequestType == RequestType.UpdateMessage).HandleAsync(updateMessageRequest);
                
                break;
        }
    }
}