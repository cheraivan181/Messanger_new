using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Domain.Responses;
using Core.MessageServices.Services.Implementations.Handlers.Interfaces;
using Core.MessageServices.Services.Interfaces;

namespace Core.MessageServices.Services.Implementations.Handlers.Implementations;

public class ErrorMessageHandler : IErrorMessageHandler
{
    private readonly ISenderService _senderService;
    private readonly IPrepereMessagesToSendService _prepereMessagesToSendService;
    
    public ErrorMessageHandler(ISenderService senderService,
        IPrepereMessagesToSendService prepereMessagesToSendService)
    {
        _senderService = senderService;
        _prepereMessagesToSendService = prepereMessagesToSendService;
    }
    
    public async Task HandleAsync(Guid actionId, Guid userId, string connectionId, string text)
    {
        var errorResponse = new ErrorResponse()
        {
            Message = text,
            ActionId = actionId
        };

        var messageToSend = await _prepereMessagesToSendService.BuildMessageForSingleConnection(errorResponse,
            userId, connectionId, ResponseAction.Error, ResponseCode.GlobalError);

        await _senderService.SendMessageToUserAsync(messageToSend);
    }
}