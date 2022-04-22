using Front.Clients.Interfaces;
using Front.Services.Interfaces.Crypt;
using Front.Services.Interfaces.Dialogs;

namespace Front.Services.Implementations.Crypt
{
    public class AesService : IAesCryptService
    {
        private readonly ICryptClient _cryptClient;
        private readonly IDialogManagerService _dialogService;

        public AesService(ICryptClient cryptClient,
            IDialogManagerService dialogService)
        {
            _cryptClient = cryptClient;
            _dialogService = dialogService;
        }

        public async Task<string> CryptText(Guid dialogId, string textToCrypt)
        {
            var dialogs = await _dialogService.GetDialogsAsync();
            var dialog = dialogs.Single(x => x.DialogId == dialogId);
            var iv = await _cryptClient.GetAesKetAndIvAsync();
            var result = await _cryptClient.AesCryptAsync(dialog.CypherKey, iv.SucessResponse.Response.Iv, textToCrypt);

            return result.SucessResponse.Response.CryptedText;
        }

        public async Task<string> DecryptText(Guid dialogId,string iv, string cipherText)
        {
            var dialogs = await _dialogService.GetDialogsAsync();
            var dialog = dialogs.Single(x => x.DialogId == dialogId);
            var result = await _cryptClient.AesCryptAsync(dialog.CypherKey, iv, cipherText);

            return result.SucessResponse.Response.CryptedText;
        }
    }
}
