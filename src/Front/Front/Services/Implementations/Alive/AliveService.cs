using Front.Clients.Interfaces;
using Front.Services.Interfaces.Alive;
using Front.Store.Implementations;

namespace Front.Services.Implementations.Alive
{
    public class AliveService : IAliveService
    {
        private readonly IAliveClient _aliveClient;
        private readonly IGlobalVariablesStoreService _globalVariableStoreService;

        public AliveService(IAliveClient aliveClient,
            IGlobalVariablesStoreService globalVariableStoreService)
        {
            _aliveClient = aliveClient;
            _globalVariableStoreService = globalVariableStoreService;
        }

        public async Task<bool> IsApiAliveAsync()
        {
            var isAliveResponse = await _aliveClient.IsAliveAsync();
            if (!isAliveResponse.IsSucess)
            {
                await _globalVariableStoreService.SetIsAliveAsync(true);
                return false;
            }

            return true;
        }
    }
}
