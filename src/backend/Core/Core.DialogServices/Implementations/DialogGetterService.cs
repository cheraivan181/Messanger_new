using Core.DialogServices.Domain;
using Core.DialogServices.Interfaces;
using Core.Repositories.Interfaces;

namespace Core.DialogServices.Implementations;

public class DialogGetterService : IDialogGetterService
{
    private readonly IDialogRepository _dialogRepository;
    private readonly IDialogCacheService _dialogCacheService;

    public DialogGetterService(IDialogRepository dialogRepository,
        IDialogCacheService dialogCacheService)
    {
        _dialogRepository = dialogRepository;
        _dialogCacheService = dialogCacheService;
    }
    
    
    public async Task<List<DialogCacheModel>> GetDialogsAsync(Guid userId)
    {
        var dialogsFromCache = await _dialogCacheService.GetDialogsFromCacheAsync(userId);
        if (dialogsFromCache == null)
        {
            var dialogsFromDatabase = await _dialogRepository.GetUserDialogsAsync(userId);
            dialogsFromCache = dialogsFromDatabase.Select(x => new DialogCacheModel()
            {
                DialogId = x.Id
            }).ToList();

            _dialogCacheService.SetDialogsInCache(userId, dialogsFromDatabase);
            return dialogsFromDatabase.Select(x => new DialogCacheModel()
            {
                DialogId = x.Id
            }).ToList();
        }

        return dialogsFromCache;
    }
}