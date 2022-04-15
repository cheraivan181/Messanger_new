using Blazored.LocalStorage;
using Front.Domain.Session;
using Front.Json;
using Front.Services.Interfaces.Sessions;

namespace Front.Services.Implementations.Sessions
{
    public class SessionGetterService : ISessionGetterService
    {
        private readonly ILocalStorageService _localStorageService;

        public SessionGetterService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<SessionModel> GetSessionAsync()
        {
            var sessionValue = await _localStorageService.GetItemAsStringAsync(Constants.SessionName);
            if (string.IsNullOrEmpty(sessionValue))
                return null;

            return sessionValue.FromJson<SessionModel>();
        }
    }
}
