using Blazored.SessionStorage;
using Front.Store.Implementations;

namespace Front.Store.Services
{
    public class GlobalVariablesStoreService : IGlobalVariablesStoreService
    {
        private readonly ISessionStorageService _sessionStorageService;

        public GlobalVariablesStoreService(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }


        public ValueTask SetIsAliveAsync(bool isAlive) =>
            _sessionStorageService.SetItemAsStringAsync("isAlive", isAlive.ToString());

        public ValueTask SetIsAuthenticated(bool isAuth) =>
            _sessionStorageService.SetItemAsStringAsync("isAuth", isAuth.ToString());

        public ValueTask SetIsGlobalError(bool isGlobalError) =>
            _sessionStorageService.SetItemAsStringAsync("isGlobalError", isGlobalError.ToString());

        public async ValueTask<bool> GetIsAliveAsync()
        {
            var result = await GetBoolConfig("isAlive");
            return result;
        }

        public async Task<bool> GetIsAuthenticated()
        {
            var result = await GetBoolConfig("isAuth");
            return result;
        }

        public async Task<bool> GetIsGlobalError()
        {
            var result = await GetBoolConfig("isGlobalError");
            return result;
        }

        private async Task<bool> GetBoolConfig(string name)
        {
            var result = await _sessionStorageService.GetItemAsStringAsync(name);
            if (string.IsNullOrEmpty(result)
                || !bool.TryParse(result, out bool res))
                return true;

            return res;
        }
    }
}
