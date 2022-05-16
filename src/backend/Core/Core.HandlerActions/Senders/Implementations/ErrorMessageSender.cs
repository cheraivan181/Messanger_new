using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Domain.Interfaces;
using Core.CryptProtocol.Domain.Responses;
using Core.MessageServices.Services.Implementations.Senders.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Core.SenderServices.Domain;

namespace Core.MessageServices.Services.Implementations.Senders.Implementations;

public class ErrorMessageSender : ISender
{
    private readonly IPrepereMessagesToSendService _prepereMessagesToSendService;
    private readonly ISenderService _senderService;

    public ErrorMessageSender(IPrepereMessagesToSendService prepereMessagesToSendService,
        ISenderService senderService)
    {
        _prepereMessagesToSendService = prepereMessagesToSendService;
        _senderService = senderService;
    }
    
    public ResponseAction ResponseAction => ResponseAction.Error;
    public int ProtocolVersion => 1;
    
    public async Task SendAsync(Guid userId, IResponseProtocolMessage responseProtocolMessage, string connectionId = null)
    {
        var errorResponse = responseProtocolMessage as ErrorResponse;

        MessageToSendInNetwork messageToSendInNetwork;

        if (!string.IsNullOrEmpty(connectionId))
        {
            messageToSendInNetwork = await _prepereMessagesToSendService.BuildMessageForSingleConnection(errorResponse,
                userId, connectionId, ResponseAction.Error, errorResponse.ResponseCode);
        }
        else
        {
            messageToSendInNetwork = await _prepereMessagesToSendService.BuildMessageToSendResponseAsync(errorResponse,
                userId, ResponseAction.Error, errorResponse.ResponseCode);
        }

        await _senderService.SendMessageToUserAsync(messageToSendInNetwork);
    }
}