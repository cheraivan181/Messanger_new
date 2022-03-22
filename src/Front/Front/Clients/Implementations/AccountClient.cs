using System.Net.Http;

namespace Front.Clients.Implementations
{
    public class AccountClient
    {
        private readonly HttpClient _httpClient;

        public AccountClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(); 
        }

        public async Task SignInAsync() { }
        public async Task SignUpAsync() { }
        public async Task RerfreshTokenAsync() { }
    }
}
