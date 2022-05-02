using Core.DbModels;

namespace Core.MessageServices.Services.Interfaces;

public interface IMessageCacheService
{
    Task<List<string>> GetMessagesFromCacheAsync(Guid dialogId);
    Task AddMessageInCacheAsync(Guid dialogId, Message message);
    Task UpdateMessageAsync(Guid dialogId, Message message);
}