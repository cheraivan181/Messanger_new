using Blazored.LocalStorage;
using Front.Clients.Interfaces;
using Front.Services.Interfaces.Crypt;

namespace Front.Services.Implementations.Crypt
{
    public class RsaService : IRsaService
    {
        private readonly ICryptClient _cryptClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly ILogger<RsaService> _logger;

        public RsaService(ICryptClient cryptClient,
            ILocalStorageService localStoragesService,
            ILoggerFactory loggerFactory)
        {
            _localStorageService = localStoragesService;
            _cryptClient = cryptClient;
            _logger = loggerFactory.CreateLogger<RsaService>();
        }

        public async Task<(string privateKey, string publicKey)> GetRsaKeysAsync()
        {
            var result = await _cryptClient.GetRsaKeysAsync();
            if (!result.IsSucess)
            {
                _logger.LogError($"Cannot get rsa keys. Error: {result.ErrorResponse}");
                throw new Exception("Cannot get keys");
            }

            return (result.SucessResponse.Response.PrivateKey, result.SucessResponse.Response.PublicKey);
        }
    }
}
