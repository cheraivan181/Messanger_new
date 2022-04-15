using Blazored.LocalStorage;
using Front.Clients.Interfaces;
using Front.Domain.Session;
using Front.Json;
using Front.Services.Interfaces.Auth;
using Front.Services.Interfaces.Crypt;
using Front.Services.Interfaces.Sessions;
using System.Text.Json;

namespace Front.Services.Implementations.Sessions
{
    public class SessionService : ISessionService
    {
        private readonly IRsaService _rsaService;
        private readonly ILocalStorageService _localStorageService;
        private readonly ISessionClient _sessionClient;
        private readonly IAuthService _authService;

        public SessionService(IRsaService rsaService,
            ILocalStorageService localStorageService,
            ISessionClient sessionClient,
            IAuthService authService)
        {
            _rsaService = rsaService;
            _localStorageService = localStorageService;
            _sessionClient = sessionClient;
            _authService = authService;
        }

        public async Task<bool> CreateSessionService()
        {
            var rsaKeys = await _rsaService.GetRsaKeysAsync();
            var createSessionResponse = await _sessionClient.CreateSessionAsync(rsaKeys.publicKey);
            if (!createSessionResponse.IsSucess)
                return false;

            var session = new SessionModel()
            {
                ClientPublicKey = rsaKeys.publicKey,
                ClientPrivateKey = rsaKeys.privateKey,
                ServerPublicKey = createSessionResponse.SucessResponse.Response.ServerPublicKey,
                SessionId = createSessionResponse.SucessResponse.Response.SessionId
            };

            await _localStorageService.SetItemAsStringAsync(Constants.SessionName, session.ToJson());

            if (createSessionResponse.SucessResponse.Response.IsNeedUpdateToken)
                await _authService.UpdateRefreshTokenAsync();
            return true;
        }

        public async Task<bool> IsNeedCreateSessionAsync()
        {
            var savedSession = await _localStorageService.GetItemAsStringAsync(Constants.SessionName);
            return string.IsNullOrEmpty(savedSession);
        }
    }
}
