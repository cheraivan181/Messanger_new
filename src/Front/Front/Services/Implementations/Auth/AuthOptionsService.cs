using Front.Clients.Interfaces;
using Front.Domain.Responses;
using Front.Servives.Interfaces.Auth;

namespace Front.Servives.Implementations.Auth
{
    public class AuthOptionsService : IAuthOptionsService
    {
        private readonly IAccountClient _accountClient;
        private static AuthOptions _authOptions;

        private readonly ILogger<AuthOptionsService> _logger;

        public AuthOptionsService(IAccountClient accountClient,
            ILoggerFactory loggerFactory)
        {
            _accountClient = accountClient;
            _logger = loggerFactory.CreateLogger<AuthOptionsService>();
        }

        public async ValueTask<AuthOptions> GetAuthOptionsAsync()
        {
            if (_authOptions != null)
                return _authOptions;

            _logger.LogInformation("Try to get authOptions info from server");

            var authOptions = await _accountClient.GetAuthOptionsAsync();
            if (!authOptions.IsSucess)
            {
                _logger.LogError($"Cannot get authoptions from server");
                throw new Exception($"Server error. Cannot get authinformation from the server");
            }

            _logger.LogDebug($"Get auth options is complete");
            _authOptions = authOptions.SucessResponse.Response;

            return _authOptions;
        }
    }
}
