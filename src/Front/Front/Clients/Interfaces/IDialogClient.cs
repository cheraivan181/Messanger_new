using Front.ClientsDomain.Responses.Dialog;
using Front.Domain.Responses.Base;

namespace Front.Clients.Interfaces
{
    public interface IDialogClient
    {
        Task<RestClientResponse<GetDialogResponse>> GetDialogsAsync();
        Task<RestClientResponse<CreateDialogResponse>> CreateDialogAsync(Guid requestUserId);
    }
}
