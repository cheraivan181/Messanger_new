using Core.DbModels;

namespace Core.Repositories.Interfaces;

public interface IDialogRequestRepository
{
    Task<Guid> CreateDialogRequestAsync(Guid ownerUserId, Guid requestUserId);
    Task<DialogRequest> GetDialogRequestAsync(Guid ownerUserId, Guid requestUserId);
    Task ProcessDialogRequestAsync(Guid ownerUserId, Guid requestUserId, bool isAccepted);
}