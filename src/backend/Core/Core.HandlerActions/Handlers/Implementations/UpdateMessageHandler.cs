using Core.CryptProtocol.Domain;
using Core.CryptProtocol.Domain.Base;
using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Domain.Interfaces;
using Core.CryptProtocol.Domain.Responses;
using Core.MessageServices.Services.Implementations.Handlers.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Core.Repositories.Interfaces;
using Core.Utils;
using Serilog;

namespace Core.MessageServices.Services.Implementations.Handlers.Implementations;

public class UpdateMessageHandler : IHandler
{
    private readonly IMessageRepository _messageRepository;
    private readonly IDialogRepository _dialogRepository;

    private readonly ISenderService _senderService;
    private readonly IMessageCacheService _messageCacheService;
    private readonly IPrepereMessagesToSendService _prepereMessagesToSendService;

    public RequestType RequestType => RequestType.UpdateMessage;
    public int ProtocolVersion => 1;

    public UpdateMessageHandler(IMessageRepository messageRepository,
        IDialogRepository dialogRepository,
        ISenderService senderService,
        IMessageCacheService messageCacheService,
        IPrepereMessagesToSendService prepereMessagesToSendService)
    {
        _messageRepository = messageRepository;
        _dialogRepository = dialogRepository;
        _senderService = senderService;
        _messageCacheService = messageCacheService;
        _prepereMessagesToSendService = prepereMessagesToSendService;
    }

    public async Task HandleAsync(IRequestProtocolMessage requestProtocolMessage)
    {
        var request = requestProtocolMessage as UpdateMessageResquest;
        
        var message = await _messageRepository.GetMessageAsync(request.MessageId);
        if (message == null)
        {
            Log.Error($"Cannot find messageId #({request.MessageId})");
            return;
        }

        if (request.IsDeleted)
            await _messageRepository.SetMessageDeleteAsync(request.MessageId);
        if (request.IsReaded)
            await _messageRepository.SetMessageReadAsync(request.MessageId);
        if (!string.IsNullOrEmpty(request.NewCryptedText))
            await _messageRepository.UpdateTextMessageAsync(request.MessageId, request.NewCryptedText);

        await _messageCacheService.UpdateMessageAsync(request.DialogId, message);
        
        var dialog = await _dialogRepository.GetDialogAsync(message.DialogId);

        var response = new UpdateMessageResponse()
        {
            MessageId = request.MessageId,
            DialogId = request.DialogId,
            NewText = request.NewCryptedText,
            IsReaded = request.IsReaded,
            IsDeleted = request.IsDeleted
        };

        var receiverId = CommonMethods.GetDialogId(request.SenderRequestId, dialog.User1Id, dialog.User2Id);
        
        var senderResponse = await _prepereMessagesToSendService
            .BuildMessageToSendResponseAsync(response, request.SenderRequestId, ResponseAction.UpdateMessage, ResponseCode.Sucess);
        var receiverResponse = await _prepereMessagesToSendService
            .BuildMessageToSendResponseAsync(response, receiverId, ResponseAction.UpdateMessage, ResponseCode.Sucess);

        await _senderService.SendMessageToUserAsync(senderResponse);
        await _senderService.SendMessageToUserAsync(receiverResponse);
    }
}