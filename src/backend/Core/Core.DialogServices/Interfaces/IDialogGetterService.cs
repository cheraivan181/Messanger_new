using Core.DialogServices.Domain;

namespace Core.DialogServices.Interfaces;

public interface IDialogGetterService
{
    Task<List<DialogCacheModel>> GetDialogsAsync(Guid userId);
}