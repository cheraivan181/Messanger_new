using Core.CryptProtocol.Domain;
using Core.CryptProtocol.Domain.Base;
using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Domain.Interfaces;
using Core.CryptProtocol.Domain.Responses;
using Core.DialogServices.Interfaces;
using Core.MessageServices.Services.Implementations.Handlers.Interfaces;
using Core.MessageServices.Services.Implementations.Senders.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Core.Repositories.Interfaces;

namespace Core.MessageServices.Services.Implementations.Handlers.Implementations;

public class GetDialogMessagesHandler : IHandler
{
    private readonly IMessageGetterService _messageGetterService;
    private readonly IPrepereMessagesToSendService _prepereMessagesToSendService;
    private readonly ISenderService _senderService;
    private readonly IDialogGetterService _dialogGetterService;
    private readonly ISender _errorSender;

    public RequestType RequestType => RequestType.GetMessages;
    public int ProtocolVersion => 1;

    public GetDialogMessagesHandler(IMessageGetterService messageGetterService,
        IPrepereMessagesToSendService prepereMessagesToSendService,
        ISenderService senderService,
        IDialogGetterService dialogGetterService,
        IEnumerable<ISender> senders)
    {
        _messageGetterService = messageGetterService;
        _prepereMessagesToSendService = prepereMessagesToSendService;
        _senderService = senderService;
        _dialogGetterService = dialogGetterService;
        _errorSender = senders.Single(x => x.ResponseAction == ResponseAction.Error);
    }

    public async Task HandleAsync(IRequestProtocolMessage requestProtocolMessage)
    {
        var request = requestProtocolMessage as GetMessageRequest;
        var dialogs = await _dialogGetterService.GetDialogsAsync(request.UserId);

        if (!dialogs.Any(x => x.DialogId == request.DialogId))
        {
            var errorResponse = new ErrorResponse()
            {
                Message = request.DialogId.ToString(),
                ActionId = request.ActionId,
                ResponseCode = ResponseCode.NotAvailableDialog
            };
            
            await _errorSender.SendAsync(request.UserId, errorResponse, request.ConnectionId);
            
            return;    
        }
        
        await foreach (var messageList in _messageGetterService.GetMessageListAsync(request.DialogId, request.Page))
        {
            var response = await _prepereMessagesToSendService.BuildMessageForSingleConnection(messageList,
               request.UserId, request.ConnectionId, ResponseAction.GetDialogMessages, ResponseCode.Sucess);
            await _senderService.SendMessageToUserAsync(response);
        }
    }
}