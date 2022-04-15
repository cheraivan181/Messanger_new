using Front.ClientsDomain.Responses.Dialog;

namespace Front.Services.Interfaces.Dialogs
{
    public interface IDialogManagerService
    {
        ValueTask<List<GetDialogResponse.Dialog>> GetDialogsAsync();
        Task<List<GetDialogResponse.Dialog>> CreateAndGetDialogsAsync(Guid userId, string userName);
    }
}
