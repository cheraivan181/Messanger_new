using Front.Clients.Interfaces;
using Front.Services.Interfaces.Alive;

namespace Front.Services.Implementations.Alive
{
    public class AliveService : IAliveService
    {
        private readonly IAliveClient _aliveClient;

        public AliveService(IAliveClient aliveClient)
        {
            _aliveClient = aliveClient;
        }

        public async Task<bool> IsApiAliveAsync()
        {
            var isAliveResponse = await _aliveClient.IsAliveAsync();
            if (!isAliveResponse.IsSucess)
            {
                GlobalStorage.IsAlive = false;
                return false;
            }

            return true;
        }
    }
}
