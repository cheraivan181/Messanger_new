using Front.Clients.Interfaces;
using Front.ClientsDomain.Responses.Dialog;
using Front.Services.Interfaces.Crypt;
using Front.Services.Interfaces.Dialogs;

namespace Front.Services.Implementations.Dialogs
{
    public class DialogService : IDialogManagerService
    {
        private readonly IDialogClient _dialogClient;
        private readonly IRsaService _rsaService;
        

        private static List<GetDialogResponse.Dialog> Dialogs = new List<GetDialogResponse.Dialog>();

        public DialogService(IDialogClient dialogClient,
            IRsaService rsaService)
        {
            _dialogClient = dialogClient;
            _rsaService = rsaService;
        }

        public async ValueTask<List<GetDialogResponse.Dialog>> GetDialogsAsync()
        {
            if (Dialogs.Count != 0)
            {
                return Dialogs;
            }

            var response = await _dialogClient.GetDialogsAsync();

            if (!response.IsSucess)
            {
                GlobalStorage.IsGlobalError = true;
                return Dialogs;
            }

            foreach (var dialog in response.SucessResponse.Response.Dialogs)
            {
                var cypherKey = await _rsaService.DecryptTextAsync(dialog.CypherKey);
                var iv = await _rsaService.DecryptTextAsync(dialog.IV);

                Dialogs.Add(new GetDialogResponse.Dialog(dialog.UserId, dialog.DialogId, dialog.UserName, cypherKey, iv, dialog.IsConfirmDialog));
            }

            return Dialogs;
        }

        public async Task<List<GetDialogResponse.Dialog>> CreateAndGetDialogsAsync(Guid userId, string userName)
        {
            var response = await _dialogClient.CreateDialogAsync(userId);

            if (!response.IsSucess)
            {
                GlobalStorage.IsGlobalError = true;
                return Dialogs;
            }

            var dialog = response.SucessResponse.Response;

            var key = await _rsaService.DecryptTextAsync(dialog.Key);
            var iv = await _rsaService.DecryptTextAsync(dialog.IV);

            Dialogs.Add(new GetDialogResponse.Dialog(userId, dialog.DialogId, userName, key, iv, false));
            return Dialogs;
        }
    }
}
