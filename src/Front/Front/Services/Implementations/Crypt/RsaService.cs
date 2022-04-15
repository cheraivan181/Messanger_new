using Blazored.LocalStorage;
using Front.Clients.Interfaces;
using Front.Services.Interfaces.Crypt;
using Front.Services.Interfaces.Sessions;

namespace Front.Services.Implementations.Crypt
{
    public class RsaService : IRsaService
    {
        private readonly ICryptClient _cryptClient;
        private readonly ISessionGetterService _sessionGetterService;
        private readonly ILogger<RsaService> _logger;


        public RsaService(ICryptClient cryptClient,
            ISessionGetterService sessionGetterService,
            ILoggerFactory loggerFactory)
        {
            _cryptClient = cryptClient;
            _sessionGetterService = sessionGetterService;
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


        public async Task<string> CryptTextAsync(string textToCrypt)
        {
            var session = await _sessionGetterService.GetSessionAsync();
            var result = await _cryptClient.RsaCryptAsync(session.ServerPublicKey, textToCrypt);

            return result.SucessResponse.Response.CryptedText;
        }

        public async Task<string> DecryptTextAsync(string cryptedText)
        {
            var session = await _sessionGetterService.GetSessionAsync();

            var result = await _cryptClient.RsaDecryptAsync(session.ClientPrivateKey, cryptedText);

            return result.SucessResponse.Response.DecrtyptedText;
        }
    }
}
