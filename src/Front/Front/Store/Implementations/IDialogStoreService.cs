using Front.Domain.Dialogs;

namespace Front.Store.Implementations
{
    public interface IDialogStoreService
    {
        Task<List<DialogDomainModel>> GetDialogsAsync();
        Task SetDialogsAsync(List<DialogDomainModel> models);
        Task<List<DialogDomainModel>> AddAndGetDialogsAsync(DialogDomainModel model);
    }
}
