using Core.DbModels;

namespace Core.Repositories.Interfaces;

public interface IDialogRepository
{
    Task<Guid> CreateDialogAsync(Guid user1Id, Guid user2Id, Guid dialogRequestId);
    Task<List<Dialog>> GetUserDialogsAsync(Guid userId);
}