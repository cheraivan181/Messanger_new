using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Domain.Interfaces;
using Core.CryptProtocol.Domain.Responses;
using Core.MessageServices.Services.Implementations.Senders.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Core.SenderServices.Domain;

namespace Core.MessageServices.Services.Implementations.Senders.Implementations;

public class DeliveryMessageSender : ISender
{
    private readonly ISenderService _senderService;
    private readonly IPrepereMessagesToSendService _prepereMessagesToSendService;

    public ResponseAction ResponseAction => ResponseAction.DeliveryMessage;
    public int ProtocolVersion => 1;

    public DeliveryMessageSender(ISenderService senderService,
        IPrepereMessagesToSendService prepereMessagesToSendService)
    {
        _senderService = senderService;
        _prepereMessagesToSendService = prepereMessagesToSendService;
    }
    
    
    public async Task SendAsync(Guid userId, IResponseProtocolMessage responseProtocolMessage, string connectionId = null)
    {
        var response = responseProtocolMessage as DeliveryResponse;

        MessageToSendInNetwork messageToSendInNetwork;
        
        if (!string.IsNullOrEmpty(connectionId))
        {
            messageToSendInNetwork = await _prepereMessagesToSendService.BuildMessageForSingleConnection(response,
                userId, connectionId, ResponseAction.DeliveryMessage, ResponseCode.Sucess);
        }
        else
        {
            messageToSendInNetwork = await _prepereMessagesToSendService.BuildMessageToSendResponseAsync(response,
                userId, ResponseAction.DeliveryMessage, ResponseCode.Sucess);
        }

        await _senderService.SendMessageToUserAsync(messageToSendInNetwork);
    }
}