using Front.ClientsDomain.Responses.Dialog;
using Front.Domain.Dialogs;

namespace Front.Services.Interfaces.Dialogs
{
    public interface IDialogManagerService
    {
        ValueTask<List<DialogDomainModel>> GetDialogsAsync();
        Task<DialogDomainModel> CreateAndGetDialogAsync(Guid userId, string userName); 
    }
}
