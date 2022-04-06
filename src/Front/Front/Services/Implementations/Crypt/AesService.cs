using Blazored.LocalStorage;
using Front.Clients.Interfaces;
using Front.Services.Interfaces.Crypt;

namespace Front.Services.Implementations.Crypt
{
    public class AesService : IAesCryptService
    {
        private readonly ICryptClient _cryptClient;
        private readonly ILocalStorageService _localStorageService;

        public AesService(ICryptClient cryptClient,
            ILocalStorageService localStorageService)
        {
            _cryptClient = cryptClient;
            _localStorageService = localStorageService;
        }

        public string CryptText(long dialogId, string textToCrypt)
        {
            throw new NotImplementedException();
        }

        public string DecryptText(long dialogId, string cipherText)
        {
            throw new NotImplementedException();
        }
    }
}
