using Core.MessageServices.Domain;

namespace Core.MessageServices.Services.Interfaces;

public interface IMessageGetterService
{
    IAsyncEnumerable<MessageModels> GetMessageListAsync(Guid dialogId, int page = 0);
}