using Blazored.SessionStorage;
using Front.Domain.Dialogs;
using Front.Json;
using Front.Store.Implementations;

namespace Front.Store.Services
{
    public class DialogStoreService : IDialogStoreService
    {
        private readonly ISessionStorageService _sessionStorageService;
        private const string DialogKey = "Dialogs";

        public DialogStoreService(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }

        public async Task SetDialogsAsync(List<DialogDomainModel> models)
        {
            await _sessionStorageService.SetItemAsStringAsync(DialogKey, models.ToJson());
        }

        public async Task<List<DialogDomainModel>> GetDialogsAsync()
        {
            var result = new List<DialogDomainModel>(); 
            var value = await _sessionStorageService.GetItemAsStringAsync(DialogKey);

            if (string.IsNullOrEmpty(value))
                return result;

            result = value.FromJson<List<DialogDomainModel>>();
            return result;
        }   

        public async Task<List<DialogDomainModel>> AddAndGetDialogsAsync(DialogDomainModel model)
        {
            var result = new List<DialogDomainModel>();

            var value = await _sessionStorageService.GetItemAsStringAsync(DialogKey);
            if (string.IsNullOrEmpty(value))
            {
                result.Add(model);
                await _sessionStorageService.SetItemAsStringAsync(DialogKey, result.ToJson());

                return result;
            }

            result = value.FromJson<List<DialogDomainModel>>();
            if (result.Any(x => x.DialogId == model.DialogId))
                return result;

            result.Add(model);

            await _sessionStorageService.SetItemAsStringAsync(DialogKey, result.ToJson());
            return result;
        }
    }
}
