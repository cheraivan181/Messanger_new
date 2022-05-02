﻿using Core.CryptProtocol.Domain;
using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Domain.Responses;
using Core.CryptProtocol.Services.Interfaces;
using Core.MessageServices.Services.Implementations.Handlers.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Core.Repositories.Interfaces;

namespace Core.MessageServices.Services.Implementations.Handlers.Implementations;

public class DirectMessageHandler : IDirectMessageHandler
{
    private readonly IMessageRepository _messageRepository;
    private readonly ISenderService _senderService;
    private readonly IMessageCacheService _messageCacheService;
    private readonly IPrepereMessagesToSendService _prepereMessageToSendService;
    
    public DirectMessageHandler(IResponseBuilder responseBuilder,
        ISenderService senderService,
        IMessageCacheService messageCacheService,
        IPrepereMessagesToSendService prepereMessageToSendService)
    {
        _senderService = senderService;
        _messageCacheService = messageCacheService;
        _prepereMessageToSendService = prepereMessageToSendService;
    }
    
    public async Task HandleAsync(SendDirectMessageRequest request)
    {
        // message was saved. Send response about delivery
        await _messageRepository.AddMessageAsync(request.MessageId, request.Text, request.DialogId);
        var directMessageResponse = new SendDirectMessageResponse()
        {
            MessageId = request.MessageId,
            CryptedText = request.Text,
            AnswerMessageId = request.AnswerMessageId,
            IsReaded = false,
            IsDeleted = false,
            Date = DateTime.Now,
            ToId = request.ToId,
            FromId = request.FromId,
        };

        var senderResponse = await
            _prepereMessageToSendService.BuildMessageToSendResponseAsync(directMessageResponse,
                request.FromId, ResponseAction.DirectMessage, ResponseCode.Sucess);
        var receiverResponse = await
            _prepereMessageToSendService.BuildMessageToSendResponseAsync(directMessageResponse,
                request.ToId, ResponseAction.DirectMessage, ResponseCode.Sucess);

        await _messageCacheService.AddMessageInCacheAsync(request.DialogId, new DbModels.Message()
        {
            CryptedText = request.Text,
            AnswerMessageId = request.AnswerMessageId,
            IsReaded = false,
            IsDeleted = false,
            CreatedAt = DateTime.Now,
            DialogId = request.DialogId
        });
        
        
        await _senderService.SendMessageToUserAsync(senderResponse);
        await _senderService.SendMessageToUserAsync(receiverResponse);
    }
}