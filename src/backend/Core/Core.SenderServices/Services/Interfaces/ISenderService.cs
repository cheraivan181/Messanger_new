using Core.SenderServices.Domain;

namespace Core.MessageServices.Services.Interfaces;

public interface ISenderService
{
    Task SendMessageToUserAsync(MessageToSendInNetwork messageToSend, bool isSendMessageToSpecialChannel = false);
}