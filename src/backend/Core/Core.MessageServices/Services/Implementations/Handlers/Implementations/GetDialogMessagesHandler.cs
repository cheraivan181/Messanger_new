using Core.CryptProtocol.Domain;
using Core.CryptProtocol.Domain.Base.Responses;
using Core.MessageServices.Services.Implementations.Handlers.Interfaces;
using Core.MessageServices.Services.Interfaces;

namespace Core.MessageServices.Services.Implementations.Handlers.Implementations;

public class GetDialogMessagesHandler : IGetDialogMessageHandler
{
    private readonly IMessageGetterService _messageGetterService;
    private readonly IPrepereMessagesToSendService _prepereMessagesToSendService;
    private readonly ISenderService _senderService;
    
    public GetDialogMessagesHandler(IMessageGetterService messageGetterService,
        IPrepereMessagesToSendService prepereMessagesToSendService,
        ISenderService senderService)
    {
        _messageGetterService = messageGetterService;
        _prepereMessagesToSendService = prepereMessagesToSendService;
        _senderService = senderService;
    }

    public async Task HandleAsync(GetMessageRequest request)
    {
        await foreach (var messageList in _messageGetterService.GetMessageListAsync(request.DialogId, request.Page))
        {
            var response = await _prepereMessagesToSendService.BuildMessageForSingleConnection(messageList,
               request.UserId, request.ConnectionId, ResponseAction.GetDialogMessages, ResponseCode.Sucess);
            await _senderService.SendMessageToUserAsync(response);
        }
    }
}