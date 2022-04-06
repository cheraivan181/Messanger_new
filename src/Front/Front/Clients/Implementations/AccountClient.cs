using Front.Clients.Interfaces;
using Front.ClientsDomain.Responses;
using Front.Domain.Requests;
using Front.Domain.Responses;
using Front.Domain.Responses.Base;

namespace Front.Clients.Implementations
{
    public class AccountClient : IAccountClient
    {
        private readonly IRestClient _restClient;
        
        public AccountClient(IRestClient restClient)
        {
            _restClient = restClient;
        }
        
        public async Task<RestClientResponse<SignInResponse>> SignInAsync(string userName, string password, long? sessionId)
        {
            var signInRequest = new SignInRequest()
            {
                UserName = userName,
                Password = password,
                SessionId = sessionId
            };

            var response = await _restClient.MakeHttpRequestAsync<SignInResponse>("Account/signin", HttpMethod.Post, data: signInRequest);
            return response;
        }

        public async Task<RestClientResponse<SignInResponse>> SignUpAsync(string userName, string password,
            string phone, string email)
        {
            var signUpRequest = new SignUpRequest()
            {
                UserName = userName,
                Password = password,
                Phone = phone,
                Email = email
            };

            var response = await _restClient
                .MakeHttpRequestAsync<SignInResponse>("Account/signup", HttpMethod.Post, data: signUpRequest);
            return response;
        }

        public async Task<RestClientResponse<SignInResponse>> UpdateRefreshTokenAsync(string refreshToken, long? sessionId)
        {
            var acessTokenUpdateRequest = new UpdateRefreshTokenRequest()
            {
                RefreshToken = refreshToken,
                SessionId = sessionId
            };

            var response = await _restClient
                .MakeHttpRequestAsync<SignInResponse>("Account/updatetoken", HttpMethod.Post, data: acessTokenUpdateRequest);

            return response;
        }

        public async Task<RestClientResponse<AuthOptions>> GetAuthOptionsAsync()
        {
            var response = await _restClient.MakeHttpRequestAsync<AuthOptions>("Account/getTokenLifeTimeOptions",
                HttpMethod.Get);
            return response;
        }

        public async Task<RestClientResponse<AuthInfoResponse>> GetAuthInfoResponseAsync()
        {
            var response = await _restClient.MakeHttpRequestAsync<AuthInfoResponse>("Account/getAuthInfo", HttpMethod.Get);
            return response;
        }
    }
}
