using Core.DbModels;

namespace Core.Repositories.Interfaces;

public interface IMessageRepository
{
    Task SetMessageReadAsync(Guid messageId);
    Task<List<Message>> GetMessageFromDialogAsync(Guid dialogId, int page = 0);
    Task AddMessageAsync(Guid messageId, string cryptedText, Guid dialogId);
    Task SetMessageDeleteAsync(Guid messageId);
    Task<Dictionary<Guid, List<Message>>> GetMessageListsAsync(Guid userId);
    Task<bool> IsMessageExistAsync(Guid messageId);
    Task UpdateTextMessageAsync(Guid messageId, string newText);
    Task<Message> GetMessageAsync(Guid messageId);
}