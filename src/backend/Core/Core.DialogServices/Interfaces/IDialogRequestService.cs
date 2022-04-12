using Core.DialogServices.Domain;

namespace Core.DialogServices.Interfaces;

public interface IDialogRequestService
{
    Task<ProcessDialogRequestResult> ProcessDialogRequestAsync(Guid ownerUserId, Guid requestUserId, bool isAccepted);
}