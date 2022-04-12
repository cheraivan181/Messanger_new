using Core.DialogServices.Domain;
using Core.DialogServices.Interfaces;
using Core.Repositories.Interfaces;
using Serilog;

namespace Core.DialogServices.Implementations;

public class DialogRequestService : IDialogRequestService
{
    private readonly IDialogRequestRepository _dialogRequestRepository;

    public DialogRequestService(IDialogRequestRepository dialogRequestRepository)
    {
        _dialogRequestRepository = dialogRequestRepository;
    }

    public async Task<ProcessDialogRequestResult> ProcessDialogRequestAsync(Guid ownerUserId, Guid requestUserId, bool isAccepted)
    {
        var result = new ProcessDialogRequestResult();
        var dialogRequest = await _dialogRequestRepository.GetDialogRequestAsync(ownerUserId, requestUserId);
        if (dialogRequest == null)
        {
            Log.Error($"Cannot find dialog request. OwnerUserId #({ownerUserId}), RequestUserId: #({requestUserId})");
            result.SetServerError();

            return result;
        }

        if (dialogRequest.IsProcessed)
        {
            Log.Error($"Error! DialogRequest #({dialogRequest.Id}) is alredy processed!");
            result.SetServerError();

            return result;
        }
        
        await _dialogRequestRepository.ProcessDialogRequestAsync(ownerUserId, requestUserId, isAccepted);
        
        if (isAccepted)
            result.Accept();
        else
            result.Reject();
        
        return result;
    }
}