using Core.DbModels;
using Core.DialogServices.Domain;

namespace Core.DialogServices.Interfaces;

public interface IDialogCacheService
{
    Task<List<DialogCacheModel>> GetDialogsFromCacheAsync(Guid userId);
    void SetDialogsInCache(Guid userId, List<Dialog> dialogs);
    Task AddDialogInCacheAsync(Guid user1Id, Guid user2Id, Guid dialogId);
}