using Core.DbModels;
using Core.MessageServices.Domain;
using Core.MessageServices.Services.Interfaces;

namespace Core.MessageServices.Services.Implementations;

public class MessageMapper : IMessageMapper
{
    public MessageModel Map(Message message)
    {
        var result = new MessageModel();
        result.MessageId = message.Id;
        result.CryptedText = message.CryptedText;
        result.IsDeleted = message.IsDeleted;
        result.IsReaded = message.IsReaded;
        result.AnswerMessageId = message.AnswerMessageId;
        result.Date = message.CreatedAt;

        return result;
    }
}