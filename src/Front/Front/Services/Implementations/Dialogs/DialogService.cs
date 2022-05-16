using Front.Clients.Interfaces;
using Front.ClientsDomain.Responses.Dialog;
using Front.Domain.Dialogs;
using Front.Services.Interfaces.Crypt;
using Front.Services.Interfaces.Dialogs;
using Front.Store.Implementations;

namespace Front.Services.Implementations.Dialogs
{
    public class DialogService : IDialogManagerService
    {
        private readonly IDialogClient _dialogClient;
        private readonly IRsaService _rsaService;
        private readonly IGlobalVariablesStoreService _globalVariablesStoreService;
        private readonly IDialogStoreService _dialogStoreService;

        public DialogService(IDialogClient dialogClient,
            IRsaService rsaService,
            IGlobalVariablesStoreService globalVariablesStoreService,
            IDialogStoreService dialogStoreService)
        {
            _dialogClient = dialogClient;
            _rsaService = rsaService;
            _globalVariablesStoreService = globalVariablesStoreService;
            _dialogStoreService = dialogStoreService;
        }

        public async ValueTask<List<DialogDomainModel>> GetDialogsAsync()
        {
            var savedDialogs = await _dialogStoreService.GetDialogsAsync();
            if (savedDialogs.Count > 0)
            {
                return savedDialogs;
            }

            var response = await _dialogClient.GetDialogsAsync();

            if (!response.IsSucess)
            {
                await _globalVariablesStoreService.SetIsGlobalError(true);
                return savedDialogs;
            }

            if (response.SucessResponse.Response.Dialogs != null)
            {
                foreach (var dialog in response.SucessResponse.Response.Dialogs)
                {
                    var cypherKey = await _rsaService.DecryptTextAsync(dialog.CypherKey);
                    var currentDialog = new DialogDomainModel();
                    currentDialog.SetDialogDetails(dialog.UserId, dialog.DialogId, dialog.UserName,
                        cypherKey, dialog.IsConfirmDialog,
                        dialog.Email, dialog.PhoneNumber, dialog.DialogCreateDate,
                        dialog.LastActivity);

                    savedDialogs = await _dialogStoreService.AddAndGetDialogsAsync(currentDialog);
                }
            }

            return savedDialogs;
        }

        public async Task<DialogDomainModel> CreateAndGetDialogAsync(Guid userId, string userName)
        {
            var response = await _dialogClient.CreateDialogAsync(userId);

            if (!response.IsSucess)
            {
                await _globalVariablesStoreService.SetIsGlobalError(true);
                return null;
            }

            var dialog = response.SucessResponse.Response;

            var key = await _rsaService.DecryptTextAsync(dialog.Key);
            var newDialog = new DialogDomainModel();

            newDialog.SetDialogRequestDetails(userId, dialog.DialogId, userName, 
                key, false);

            await _dialogStoreService.AddAndGetDialogsAsync(newDialog);

            return newDialog;
        }
    }
}
